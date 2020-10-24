using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Correspondence.Service.Extensions
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }

}
