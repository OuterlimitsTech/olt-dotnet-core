using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OLT.Constants;

namespace OLT.Core
{
    public class OltInternalServerErrorObjectResult : ObjectResult
    {
        public OltInternalServerErrorObjectResult(string message) : base(new OltErrorHttp { Message = message })
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

        public OltInternalServerErrorObjectResult() : this(OltAspNetCoreDefaults.InternalServerMessage)
        {
            
        }
    }
}
