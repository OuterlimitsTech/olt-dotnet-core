using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OLT.Core;

namespace OLT.AspNetCore.Versioning.Tests.Assets
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api/api-version")]
    public class TestVersionController : ControllerBase
    {

        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [HttpGet, Route("one")]
        public ActionResult ApiVersionOne()
        {
            return Ok(new { id = Faker.RandomNumber.Next() });
        }

        [ApiVersion("2.0")]
        [HttpGet, Route("two")]
        public ActionResult ApiVersionTwo()
        {
            return Ok(new { id = Faker.RandomNumber.Next() });
        }

    }
}
