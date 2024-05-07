using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public interface IUserService : IOltEntityService<UserEntity>
    {
        IQueryable<UserEntity> GetRepository();

        TModel GetSafeTest<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new();
        TModel GetSafeTest<TModel>(Expression<Func<UserEntity, bool>> predicate) where TModel : class, new();
        TModel GetSafeTest<TModel>(bool includeDeleted, params IOltSearcher<UserEntity>[] searchers) where TModel : class, new();

        Task<TModel> GetSafeTestAsync<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new();
        Task<TModel> GetSafeTestAsync<TModel>(Expression<Func<UserEntity, bool>> predicate) where TModel : class, new();
        Task<TModel> GetSafeTestAsync<TModel>(bool includeDeleted, params IOltSearcher<UserEntity>[] searchers) where TModel : class, new();
    }
}
