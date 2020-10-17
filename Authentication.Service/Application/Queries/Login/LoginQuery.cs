using Authentication.Service.Models.Auth;
using MediatR;

namespace Authentication.Service.Query.Login
{
    public class LoginQuery : IRequest<User>
    {
        public string UserName { get; set; }
        public string AppName { get; set; }
    }
}