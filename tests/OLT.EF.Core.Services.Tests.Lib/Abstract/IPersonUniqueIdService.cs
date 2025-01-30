using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Lib.Abstract;

public interface IPersonUniqueIdService : IOltEntityUniqueIdService<PersonEntity>
{
    TModel GetSafeTest<TModel>(Guid uid) where TModel : class, new();
    Task<TModel> GetSafeTestAsync<TModel>(Guid uid) where TModel : class, new();
}
