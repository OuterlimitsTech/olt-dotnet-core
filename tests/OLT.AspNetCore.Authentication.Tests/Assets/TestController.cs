using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.AspNetCore.Authentication.Tests.Assets
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api")]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet, Route("")]
        public ActionResult Get()
        {
            return Ok("Result");
        }

    }
}
