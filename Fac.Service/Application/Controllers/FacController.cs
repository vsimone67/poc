using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Fac.Service.Application.Commands;

namespace Fac.Service
{
    [Route("[controller]")]
    [ApiController]
    public class FacController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FacController> _logger;

        public FacController(IMediator mediator, ILogger<FacController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("SubmitMib")]
        public async Task<ActionResult> SubmitMib()
        {
            _logger.LogDebug("Submitting MIB for processing");
            var data = await _mediator.Send(new SendMib());
            _logger.LogDebug("MIB has been sent");
            return Ok(data);
        }

        [HttpGet]
        [Route("FacCaseDecision")]
        public async Task<ActionResult> FacCaseDecision()
        {
            _logger.LogDebug("Submitting Fac case decison");
            var data = await _mediator.Send(new SendDecisionCommand());
            _logger.LogDebug("Case has been sent");
            return Ok(data);
        }

        [HttpGet]
        [Route("SubmitCase")]
        public async Task<ActionResult> SubmitCase()
        {
            _logger.LogDebug("Submitting Fac case");
            var data = await _mediator.Send(new SubmitCaseCommand());
            _logger.LogDebug("Submit case has been sent");
            return Ok(data);
        }
    }
}
