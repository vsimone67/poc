using System;
using System.Threading.Tasks;
using Authentication.Service.Query.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Authentication.Service.Controllers
{
    //[AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorizeController> _logger;
        public AuthorizeController(IMediator mediator, ILogger<AuthorizeController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("Login/{userName}/{appName}")]
        public async Task<IActionResult> Login(string userName, string appName)
        {
            _logger.LogDebug("Sending Login Query");

            var user = await _mediator.Send(new LoginQuery()
            {
                UserName = userName,
                AppName = appName
            });

            _logger.LogDebug("Login Query Ended");

            return Ok(user);
        }

        [HttpGet]
        [Route("LoginNew")]
        public async Task<IActionResult> LoginNew()
        {
            _logger.LogDebug("Sending Login Query");

            var user = await _mediator.Send(new LoginQuery()
            {
                UserName = User.Identity.Name,
                AppName = "moo"
            });

            _logger.LogDebug("Login Query Ended");

            return Ok(user);
        }
    }
}


