using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using OLT.EF.Core.Services.Tests.Lib.Abstract;

namespace OLT.EF.Core.Services.Tests.Lib.Repos;

public class PersonUniqueIdService : OltEntityUniqueIdService<TestDbContext, PersonEntity>, IPersonUniqueIdService
{
    public PersonUniqueIdService(
        IOltServiceManager serviceManager,
        TestDbContext context) : base(serviceManager, context)
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
