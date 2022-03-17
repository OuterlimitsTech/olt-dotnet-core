using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public class PersonUniqueIdService : OltEntityUniqueIdService<UnitTestContext, PersonEntity>, IPersonUniqueIdService
    {
        public PersonUniqueIdService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }
    }
}
