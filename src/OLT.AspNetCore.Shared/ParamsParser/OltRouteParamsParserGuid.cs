using System;

namespace OLT.Core
{
    public class OltRouteParamsParserGuid : OltRouteParamsParser<Guid>
    {
        public override bool TryParse(string param, out Guid value)
        {
            return Guid.TryParse(param, out value);
        }
    }



}
