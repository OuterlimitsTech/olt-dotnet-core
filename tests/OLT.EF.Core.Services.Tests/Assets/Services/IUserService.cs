using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IUserService : IOltEntityService<UserEntity>
    {
        IQueryable<UserEntity> GetRepository();
        TModel GetSafeTest<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new();
        Task<TModel> GetSafeTestAsync<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new();
    }
}
