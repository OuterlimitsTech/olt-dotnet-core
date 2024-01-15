using System;

namespace OLT.Core
{

    public class OltRouteParamsParserString : OltRouteParamsParser<string?>
    {
        public override bool TryParse(string? param, out string? value)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                value = param;
                return true;
            }

            value = null;
            return false;
        }
    }



}
