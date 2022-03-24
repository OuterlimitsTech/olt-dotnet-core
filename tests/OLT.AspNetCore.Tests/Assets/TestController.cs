using Microsoft.AspNetCore.Mvc;
using OLT.Core;

namespace OLT.AspNetCore.Tests.Assets
{

    [ApiController]
    [Produces("application/json")]
    [Route("/api")]
    public class TestController : OltApiControllerBase
    {
        [HttpGet, Route("")]
        public ActionResult Get()
        {
            var result = new
            {
                id = 1,
            };

            return Ok(result);
        }

        [HttpPost, Route("")]
        public ActionResult Post(OltErrorHttp data)
        {
            return Ok(data);
        }

        [HttpGet, Route("throw-error")]
        public ActionResult TestInternalServerError(string value)
        {
            if (value == null)
            {
                return InternalServerError();
            }
            return InternalServerError(value);
        }


        [HttpGet, Route("bad-request")]
        public ActionResult TestBadRequest(string value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            return BadRequest(value);
        }
    }
}
