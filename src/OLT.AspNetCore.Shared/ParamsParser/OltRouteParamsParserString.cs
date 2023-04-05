using System;

namespace OLT.Core
{

    public class OltRouteParamsParserString : OltRouteParamsParser<string>
    {
        public override bool TryParse(string param, out string value)
        {
            if (param.IsNotEmpty())
            {
                value = param;
                return true;
            }

            value = null;
            return false;
        }
    }



}
