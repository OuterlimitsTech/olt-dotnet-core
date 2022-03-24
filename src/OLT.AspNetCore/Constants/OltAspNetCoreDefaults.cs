using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Constants
{
    public static class OltAspNetCoreDefaults
    {
        public const string InternalServerMessage = "An error occurred";

        public static class ApiVersion
        {
            public static class ParameterName
            {
                public const string Query = "api-version";
                public const string MediaType = "v";
                public const string Header = "x-api-version";
            }
        }


    }
}
