using Microsoft.AspNetCore.Mvc;

namespace OLT.Core
{
    [ApiController]
    [Produces("application/json")]
    public abstract class OltApiControllerBase : ControllerBase
    {

        [NonAction]
        public virtual BadRequestObjectResult BadRequest(string error)
        {
            return BadRequest(new OltErrorHttp { Message = error });
        }

        [NonAction]
        public virtual OltInternalServerErrorObjectResult InternalServerError()
        {
            return new OltInternalServerErrorObjectResult();
        }
        [NonAction]
        public virtual OltInternalServerErrorObjectResult InternalServerError(string message)
        {
            return new OltInternalServerErrorObjectResult(message);
        }
    }
}
