using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public class PersonUniqueIdService : OltEntityUniqueIdService<UnitTestContext, PersonEntity>, IPersonUniqueIdService
    {
        public PersonUniqueIdService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }


        public TModel GetSafeTest<TModel>(Guid uid) where TModel : class, new()
        {
            return base.GetSafe<TModel>(uid);
        }

        public Task<TModel> GetSafeTestAsync<TModel>(Guid uid) where TModel : class, new()
        {
            return base.GetSafeAsync<TModel>(uid);
        }
    }
}
