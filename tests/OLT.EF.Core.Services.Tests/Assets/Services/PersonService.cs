using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{

    public class PersonService : OltEntityIdService<UnitTestContext, PersonEntity>, IPersonService
    {
        public PersonService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }

        public TModel GetSafeTest<TModel>(int id) where TModel : class, new()
        {
            return base.GetSafe<TModel>(id);
        }

        public Task<TModel> GetSafeTestAsync<TModel>(int id) where TModel : class, new()
        {
            return base.GetSafeAsync<TModel>(id);
        }
    }
}
