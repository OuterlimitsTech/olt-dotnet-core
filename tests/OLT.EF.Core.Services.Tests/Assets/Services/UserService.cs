using OLT.Core;
using OLT.EF.Core.Services.Tests.Assets.Entites;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace OLT.EF.Core.Services.Tests.Assets.Services
{
    public class UserService : OltEntityService<UnitTestContext, UserEntity>, IUserService
    {
        public UserService(
            IOltServiceManager serviceManager,
            UnitTestContext context) : base(serviceManager, context)
        {
        }

        public IQueryable<UserEntity> GetRepository()
        {
            return Repository;
        }

        public TModel GetSafeTest<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new()
        {
            return base.GetSafe<TModel>(searcher);
        }

        public TModel GetSafeTest<TModel>(Expression<Func<UserEntity, bool>> predicate) where TModel : class, new()
        {
            return base.GetSafe<TModel>(predicate);
        }

        public TModel GetSafeTest<TModel>(bool includeDeleted, params IOltSearcher<UserEntity>[] searchers) where TModel : class, new()
        {
            return base.GetSafe<TModel>(includeDeleted, searchers);
        }

        public Task<TModel> GetSafeTestAsync<TModel>(IOltSearcher<UserEntity> searcher) where TModel : class, new()
        {
            return base.GetSafeAsync<TModel>(searcher);
        }

        public Task<TModel> GetSafeTestAsync<TModel>(Expression<Func<UserEntity, bool>> predicate) where TModel : class, new()
        {
            return base.GetSafeAsync<TModel>(predicate);
        }

        public Task<TModel> GetSafeTestAsync<TModel>(bool includeDeleted, CancellationToken cancellationToken, params IOltSearcher<UserEntity>[] searchers) where TModel : class, new()
        {
            return base.GetSafeAsync<TModel>(includeDeleted, cancellationToken, searchers);
        }
    }
}
