using System.Collections.Generic;

namespace OLT.Email.Common.Tests.Assets
{
    public class TestException : OltEmailValidationException
    {
        public TestException(List<string> errors) : base(errors, "Test Validation")
        {
        }

    }
}
