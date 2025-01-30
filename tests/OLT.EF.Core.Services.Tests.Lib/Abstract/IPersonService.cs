using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;

namespace OLT.EF.Core.Services.Tests.Lib.Abstract;

public interface IPersonService : IOltEntityIdService<PersonEntity>
{
    TModel GetSafeTest<TModel>(int id) where TModel : class, new();
    Task<TModel> GetSafeTestAsync<TModel>(int id) where TModel : class, new();
}
