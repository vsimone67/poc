using Authentication.Service.Authentication;
using Authentication.Service.Models;
using Authentication.Service.Models.Auth;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Authentication.Service.Query.Login
{
    internal class LoginQueryHandler : IRequestHandler<LoginQuery, User>
    {
        private readonly IOptions<AuthorizationSettings> _settings;

        public LoginQueryHandler(IOptions<AuthorizationSettings> settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<User> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            // make async
            return await Task.Run(() =>
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Cryptography.Decrypt(_settings.Value.SecretKey)));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, request.UserName),
                     new Claim(ClaimTypes.Role,"Administrator")
            };

                var tokeOptions = new JwtSecurityToken(
                   issuer: _settings.Value.Issuer,
                   audience: _settings.Value.Audience,
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(_settings.Value.TokenExpires),
                   signingCredentials: signinCredentials
               );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                var user = new User
                {
                    Username = request.UserName,
                    Token = tokenString,
                    FirstName = "Boo",
                    LastName = "Bat"
                };

                user.Roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
                return user;
            });
        }
    }
}