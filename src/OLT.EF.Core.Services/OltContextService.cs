using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OLT.Core
{
    public abstract class OltContextService<TContext> : OltCoreService<OltEfCoreServiceManager>
        where TContext : IOltDbContext
    {
        protected OltContextService(
            IOltServiceManager serviceManager,
            TContext context) : base(serviceManager)
        {
            Context = context;
        }

        protected virtual TContext Context { get; private set; }

        #region [ Save Changes ]

        protected virtual int SaveChanges() => Context.SaveChanges();
        protected virtual async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync(CancellationToken.None);

        #endregion

        #region [ Queryable Methods ]

        protected virtual IQueryable<TEntity> InitializeQueryable<TEntity>(bool includeDeleted) where TEntity : class, IOltEntity
        {
            return Context.InitializeQueryable<TEntity>(includeDeleted);
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TEntity : class, IOltEntity
        {
            var queryable = InitializeQueryable<TEntity>(includeDeleted);
            searchers.ToList().ForEach(builder =>
            {
                queryable = builder.BuildQueryable(queryable);
            });
            return queryable;
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, params IOltSearcher<TEntity>[] searchers) where TEntity : class, IOltEntity
        {
            return orderBy(GetQueryable(includeDeleted, searchers));
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(bool includeDeleted) where TEntity : class, IOltEntity
        {
            return GetQueryable(new OltSearcherGetAll<TEntity>(includeDeleted));
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(IOltSearcher<TEntity> queryBuilder, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy)
            where TEntity : class, IOltEntity
        {
            return orderBy(GetQueryable(queryBuilder));
        }

        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(IOltSearcher<TEntity> searcher)
            where TEntity : class, IOltEntity
        {
            return searcher.BuildQueryable(InitializeQueryable<TEntity>(searcher.IncludeDeleted));
        }

        #endregion

        #region [ Get All ]

        protected virtual IEnumerable<TModel> GetAll<TEntity, TModel>(IOltSearcher<TEntity> searcher)
           where TEntity : class, IOltEntity
           where TModel : class, new()
            => this.GetAll<TEntity, TModel>(this.GetQueryable(searcher));

        protected virtual IEnumerable<TModel> GetAll<TEntity, TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy)
            where TEntity : class, IOltEntity
            where TModel : class, new()
            => this.GetAll<TEntity, TModel>(this.GetQueryable(searcher, orderBy));

        protected virtual IEnumerable<TModel> GetAll<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
            where TModel : class, new()
            => MapList<TEntity, TModel>(queryable);

        protected virtual async Task<IEnumerable<TModel>> GetAllAsync<TEntity, TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy)
            where TEntity : class, IOltEntity
            where TModel : class, new()
            => await this.GetAllAsync<TEntity, TModel>(this.GetQueryable(searcher, orderBy));

        protected virtual async Task<IEnumerable<TModel>> GetAllAsync<TEntity, TModel>(IOltSearcher<TEntity> searcher)
            where TEntity : class, IOltEntity
            where TModel : class, new()
            => await this.GetAllAsync<TEntity, TModel>(this.GetQueryable(searcher));

        protected virtual async Task<IEnumerable<TModel>> GetAllAsync<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
            where TModel : class, new()
            => await MapListAsync<TEntity, TModel>(queryable);


        #endregion

        #region [ Get ]

        protected virtual TModel Get<TEntity, TModel>(IOltSearcher<TEntity> searcher)
            where TModel : class, new()
            where TEntity : class, IOltEntity
            => MapFirst<TEntity, TModel>(this.GetQueryable(searcher));

        protected virtual TModel Get<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
            => MapFirst<TEntity, TModel>(queryable);

        protected virtual async Task<TModel> GetAsync<TEntity, TModel>(IOltSearcher<TEntity> searcher)
            where TModel : class, new()
            where TEntity : class, IOltEntity
            => await MapFirstAsync<TEntity, TModel>(this.GetQueryable(searcher));

        protected virtual async Task<TModel> GetAsync<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
            => await MapFirstAsync<TEntity, TModel>(queryable);


        #endregion

        #region [ Mark Deleted ]

        protected virtual bool MarkDeleted<TEntity>(TEntity entity)
            where TEntity : class, IOltEntity
        {
            if (entity is IOltEntityDeletable deletableEntity)
            {
                deletableEntity.DeletedOn = DateTimeOffset.Now;
                deletableEntity.DeletedBy = Context.AuditUser;
                SaveChanges();
                return true;
            }

            throw new InvalidCastException($"Unable to cast to {nameof(IOltEntityDeletable)}");

        }

        protected virtual async Task<bool> MarkDeletedAsync<TEntity>(TEntity entity)
            where TEntity : class, IOltEntity
        {
            if (entity is IOltEntityDeletable deletableEntity)
            {
                deletableEntity.DeletedOn = DateTimeOffset.Now;
                deletableEntity.DeletedBy = Context.AuditUser;
                await SaveChangesAsync();
                return true;
            }

            throw new InvalidCastException($"Unable to cast to {nameof(IOltEntityDeletable)}");

        }


        #endregion

        #region [ Count ]

        protected virtual int Count<TEntity>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
        {
            return queryable.Count();
        }

        protected virtual async Task<int> CountAsync<TEntity>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
        {
            return await queryable.CountAsync();
        }

        #endregion

        #region [ Any ]

        protected virtual bool Any<TEntity>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
        {
            return queryable.Any();
        }

        protected virtual async Task<bool> AnyAsync<TEntity>(IQueryable<TEntity> queryable)
            where TEntity : class, IOltEntity
        {
            return await queryable.AnyAsync();
        }

        #endregion

        #region [ Mapping ]

        protected virtual IOltPaged<TModel> MapPaged<TEntity, TModel>(IQueryable<TEntity> queryable, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                queryable = orderBy == null ? ServiceManager.AdapterResolver.ApplyDefaultOrderBy<TEntity, TModel>(queryable) : orderBy(queryable);
                var mapped = ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable, config => config.DisableBeforeMap = true);
                return OltPagedExtensions.ToPaged(mapped, pagingParams);
            }
            throw new OltAdapterNotFoundException(OltAdapterExtensions.BuildAdapterName<TEntity, TModel>());
        }

        protected virtual async Task<IOltPaged<TModel>> MapPagedAsync<TEntity, TModel>(IQueryable<TEntity> queryable, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                queryable = orderBy == null ? ServiceManager.AdapterResolver.ApplyDefaultOrderBy<TEntity, TModel>(queryable) : orderBy(queryable);
                var mapped = ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable, config => config.DisableBeforeMap = true);
                return await OltEfCoreQueryableExtensions.ToPagedAsync(mapped, pagingParams);
            }
            throw new OltAdapterNotFoundException(OltAdapterExtensions.BuildAdapterName<TEntity, TModel>());
        }

        protected virtual IEnumerable<TModel> MapList<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                return ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable).ToList();
            }
            var list = queryable.ToList();
            return ServiceManager.AdapterResolver.Map<TEntity, TModel>(list);
        }

        protected virtual async Task<IEnumerable<TModel>> MapListAsync<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                return await ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable).ToListAsync();
            }

            var list = await queryable.ToListAsync();
            return ServiceManager.AdapterResolver.Map<TEntity, TModel>(list);
        }

        protected virtual TModel MapFirst<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                return ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable).FirstOrDefault();
            }

            var model = new TModel();
            var entity = queryable.FirstOrDefault();
            return ServiceManager.AdapterResolver.Map(entity, model);
        }

        protected virtual async Task<TModel> MapFirstAsync<TEntity, TModel>(IQueryable<TEntity> queryable)
            where TModel : class, new()
            where TEntity : class, IOltEntity
        {
            if (ServiceManager.AdapterResolver.CanProjectTo<TEntity, TModel>())
            {
                return await ServiceManager.AdapterResolver.ProjectTo<TEntity, TModel>(queryable).FirstOrDefaultAsync();
            }

            var model = new TModel();
            var entity = await queryable.FirstOrDefaultAsync();
            return ServiceManager.AdapterResolver.Map(entity, model);
        }

        #endregion

        #region [ DB Transaction ]

        protected virtual async Task<TResult> WithDbTransactionAsync<TResult>(Func<Task<TResult>> action)
        {
            if (Context.Database.CurrentTransaction == null)
            {
                using var transaction = await Context.Database.BeginTransactionAsync();
                try
                {
                    var result = await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                return await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(Context.Database.CurrentTransaction, action);
            }
        }

        protected virtual async Task WithDbTransactionAsync(Func<Task> action)
        {
            if (Context.Database.CurrentTransaction == null)
            {
                using var transaction = await Context.Database.BeginTransactionAsync();
                try
                {
                    await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(transaction, action);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            else
            {
                await OltEntityFrameworkCoreExtensions.CreateSubTransactionAsync(Context.Database.CurrentTransaction, action);
            }
        }

        #endregion
    }
}