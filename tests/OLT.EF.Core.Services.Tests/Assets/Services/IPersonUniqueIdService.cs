using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Threading.Tasks;
using System;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IPersonUniqueIdService : IOltEntityUniqueIdService<PersonEntity>
    {
        TModel GetSafeTest<TModel>(Guid uid) where TModel : class, new();
        Task<TModel> GetSafeTestAsync<TModel>(Guid uid) where TModel : class, new();
    }
}
