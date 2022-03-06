using Microsoft.AspNetCore.Mvc;
using OLT.Core;
using System;
using System.Collections.Generic;

namespace OLT.AspNetCore.Serilog.Tests
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api")]
    public class TestController : ControllerBase
    {
        [HttpGet, Route("")]
        public ActionResult GetSimple()
        {
            var result = new
            {
                id = 1,
            };

            return Ok(result);
        }

        [HttpGet, Route("throw-error")]
        public ActionResult TestInternalServerError()
        {
            throw new Exception();
        }


        [HttpGet, Route("bad-request")]
        public ActionResult TestBadRequest()
        {
            throw new OltBadRequestException("bad-request");
        }

        [HttpGet, Route("validation-error")]
        public ActionResult TestValidation(string value)
        {
            var errors = new List<IOltValidationError>
            {
                new OltValidationError("Error 1"),
                new OltValidationError("Error 2"),
            };

            throw new OltValidationException(errors);
        }

        [HttpGet, Route("record-not-found")]
        public ActionResult TestRecoreNotFound()
        {
            throw new OltRecordNotFoundException("record-not-found");
        }
    }
}
