namespace OLT.Core
{
    public class OltRouteParamsParserInt : OltRouteParamsParser<int>
    {
        public override bool TryParse(string? param, out int value)
        {
            return int.TryParse(param, out value);
        }
    }

}
