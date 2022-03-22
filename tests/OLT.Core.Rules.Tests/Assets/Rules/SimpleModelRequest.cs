using OLT.Core;

namespace OLT.Core.Rules.Tests.Assets.Rules
{
    public class SimpleModelRequest : OltRequest<RequestModel>
    {
        public SimpleModelRequest(RequestModel value) : base(value)
        {
        }
    }


}