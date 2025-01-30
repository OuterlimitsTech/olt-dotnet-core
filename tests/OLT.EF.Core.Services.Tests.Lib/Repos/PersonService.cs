using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.Lib.Repos;

public class PersonService : OltEntityIdService<TestDbContext, PersonEntity>, IPersonService
{
    public PersonService(
        IOltServiceManager serviceManager,
        TestDbContext context) : base(serviceManager, context)
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
