using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OLT.Core
{
    public abstract class OltEntityIdService<TContext, TEntity> : OltEntityService<TContext, TEntity>, IOltEntityIdService<TEntity>
        where TEntity : class, IOltEntityId, IOltEntity, new()
        where TContext : IOltDbContext
    {
        protected OltEntityIdService(
            IOltServiceManager serviceManager,
            TContext context) : base(serviceManager, context)
        {
        }

        #region [ Get Queryable ]

        protected virtual IQueryable<TEntity> GetQueryable(int id, bool includeDeleted = false) => InitializeQueryable(includeDeleted).Where(p => p.Id == id);

        #endregion

        #region [ Get ]

        public virtual TModel? Get<TModel>(int id, bool includeDeleted = false) where TModel : class, new() => Get<TModel>(GetQueryable(id, includeDeleted));

        public virtual async Task<TModel?> GetAsync<TModel>(int id, CancellationToken cancellationToken = default) where TModel : class, new() => await GetAsync<TModel>(GetQueryable(id, false), cancellationToken);

        public virtual async Task<TModel?> GetAsync<TModel>(int id, bool includeDeleted = false, CancellationToken cancellationToken = default) where TModel : class, new() => await GetAsync<TModel>(GetQueryable(id, includeDeleted), cancellationToken);


        public virtual TModel GetSafe<TModel>(int id, bool includeDeleted = false) where TModel : class, new() => Get<TModel>(GetQueryable(id, includeDeleted)) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        public virtual async Task<TModel> GetSafeAsync<TModel>(int id, CancellationToken cancellationToken = default) where TModel : class, new() => await GetAsync<TModel>(GetQueryable(id, false), cancellationToken) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        public virtual async Task<TModel> GetSafeAsync<TModel>(int id, bool includeDeleted = false, CancellationToken cancellationToken = default) where TModel : class, new() => await GetAsync<TModel>(GetQueryable(id, includeDeleted), cancellationToken) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        #endregion

        #region [ Build Result List ]

        /// <summary>
        /// Builds result from Adding a list of entities
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected override List<TModel> BuildResultList<TModel>(List<TEntity> entities)
        {
            var returnList = new List<TModel>();
            entities.ForEach(entity =>
            {
                returnList.Add(GetSafe<TModel>(entity.Id));
            });
            return returnList;
        }

        #endregion

        #region [ Add  ]

        public override TModel Add<TModel>(TModel model)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            Repository.Add(entity);
            SaveChanges();
            return GetSafe<TModel>(entity.Id);
        }

        public override TResponseModel Add<TResponseModel, TSaveModel>(TSaveModel model)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            Repository.Add(entity);
            SaveChanges();
            return GetSafe<TResponseModel>(entity.Id);
        }


        public override async Task<TResponseModel> AddAsync<TResponseModel, TSaveModel>(TSaveModel model, CancellationToken cancellationToken = default)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return await GetSafeAsync<TResponseModel>(entity.Id, false, cancellationToken);
        }

        public override async Task<TModel> AddAsync<TModel>(TModel model, CancellationToken cancellationToken = default)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            return await GetSafeAsync<TModel>(entity.Id, false, cancellationToken);
        }

        #endregion

        #region [ Update ]

        protected virtual void UpdateInternal<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            var queryable = GetQueryable(id);
            if (include != null)
            {
                queryable = include(queryable);
            }
            var entity = queryable.FirstOrDefault();
            ServiceManager.AdapterResolver.Map(model, entity);
            SaveChanges();
        }

        public virtual TModel Update<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TModel : class, new()
        {
            UpdateInternal(id, model, include);
            return GetSafe<TModel>(id);
        }


        public virtual TResponseModel Update<TResponseModel, TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TModel : class, new()
            where TResponseModel : class, new()
        {
            UpdateInternal(id, model, include);
            return GetSafe<TResponseModel>(id);
        }

        protected virtual async Task UpdateInternalAsync<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
        {
            var queryable = GetQueryable(id);
            if (include != null)
            {
                queryable = include(queryable);
            }
            var entity = await queryable.FirstOrDefaultAsync(cancellationToken);
            ServiceManager.AdapterResolver.Map(model, entity);
            await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TModel> UpdateAsync<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            await UpdateInternalAsync(id, model, include, cancellationToken);
            return await GetSafeAsync<TModel>(id, cancellationToken);
        }

        public virtual async Task<TResponseModel> UpdateAsync<TResponseModel, TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
            where TModel : class, new()
            where TResponseModel : class, new()
        {
            await UpdateInternalAsync(id, model, include, cancellationToken);
            return await GetSafeAsync<TResponseModel>(id, cancellationToken);
        }

        #endregion

        #region [ Soft Delete ]

        public virtual bool SoftDelete(int id)
        {
            var entity = GetQueryable(id).FirstOrDefault();
            return entity != null && MarkDeleted(entity);
        }

        public virtual async Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetQueryable(id).FirstOrDefaultAsync(cancellationToken);
            return entity != null && await MarkDeletedAsync(entity, cancellationToken);
        }

        #endregion

        #region [ Any ]

        public virtual bool Any(int id)
        {
            return Any(GetQueryable(id, true));
        }

        public virtual async Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default)
        {
            return await AnyAsync(GetQueryable(id, true), cancellationToken);
        }

        #endregion
    }
}