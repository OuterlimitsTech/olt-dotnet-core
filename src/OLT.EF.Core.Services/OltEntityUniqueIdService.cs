﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OLT.Core
{
    public abstract class OltEntityUniqueIdService<TContext, TEntity> : OltEntityService<TContext, TEntity>, IOltEntityUniqueIdService<TEntity>
        where TEntity : class, IOltEntityUniqueId, IOltEntity, new()
        where TContext : DbContext, IOltDbContext
    {
        protected OltEntityUniqueIdService(
            IOltServiceManager serviceManager, 
            TContext context) : base(serviceManager, context)
        {
        }
      

        #region [ Get Queryable ]

        protected virtual IQueryable<TEntity> GetQueryable(Guid uid) => GetQueryable(new OltSearcherGetByUid<TEntity>(uid));

        #endregion

        #region [ Get ]

        public virtual TModel? Get<TModel>(Guid uid) where TModel : class, new() => Get<TModel>(GetQueryable(uid));

        public virtual async Task<TModel?> GetAsync<TModel>(Guid uid) where TModel : class, new() => await GetAsync<TModel>(GetQueryable(uid));


        public virtual TModel GetSafe<TModel>(Guid uid) where TModel : class, new() 
            => Get<TModel>(GetQueryable(uid)) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        public virtual async Task<TModel> GetSafeAsync<TModel>(Guid uid) where TModel : class, new() 
            => await GetAsync<TModel>(GetQueryable(uid)) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

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
                returnList.Add(GetSafe<TModel>(entity.UniqueId));
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
            return GetSafe<TModel>(entity.UniqueId);
        }

        public override TResponseModel Add<TResponseModel, TSaveModel>(TSaveModel model)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            Repository.Add(entity);
            SaveChanges();
            return GetSafe<TResponseModel>(entity.UniqueId);
        }


        public override async Task<TResponseModel> AddAsync<TResponseModel, TSaveModel>(TSaveModel model)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity);
            await SaveChangesAsync();
            return await GetSafeAsync<TResponseModel>(entity.UniqueId);
        }

        public override async Task<TModel> AddAsync<TModel>(TModel model)
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity);
            await SaveChangesAsync();
            return await GetSafeAsync<TModel>(entity.UniqueId);
        }

        #endregion

        #region [ Update ]


        public virtual TModel Update<TModel>(Guid uid, TModel model)
            where TModel : class, new()
        {
            var entity = GetQueryable(uid).FirstOrDefault();
            ServiceManager.AdapterResolver.Map(model, entity);
            SaveChanges();
            return GetSafe<TModel>(uid);
        }


        public virtual TResponseModel Update<TResponseModel, TModel>(Guid uid, TModel model)
            where TModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = GetQueryable(uid).FirstOrDefault();
            ServiceManager.AdapterResolver.Map(model, entity);
            SaveChanges();
            return GetSafe<TResponseModel>(uid);
        }

        public virtual async Task<TModel> UpdateAsync<TModel>(Guid uid, TModel model)
            where TModel : class, new()
        {
            var entity = await GetQueryable(uid).FirstOrDefaultAsync();
            ServiceManager.AdapterResolver.Map(model, entity);
            await SaveChangesAsync();
            return await GetSafeAsync<TModel>(uid);
        }

        public virtual async Task<TResponseModel> UpdateAsync<TResponseModel, TModel>(Guid uid, TModel model)
            where TModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = await GetQueryable(uid).FirstOrDefaultAsync();
            ServiceManager.AdapterResolver.Map(model, entity);
            await SaveChangesAsync();
            return await GetSafeAsync<TResponseModel>(uid);
        }

        #endregion

        #region [ Soft Delete ]

        public virtual bool SoftDelete(Guid uid)
        {
            var entity = GetQueryable(uid).FirstOrDefault();
            return entity != null && MarkDeleted(entity);
        }

        public virtual async Task<bool> SoftDeleteAsync(Guid uid)
        {
            var entity = await GetQueryable(uid).FirstOrDefaultAsync();
            return entity != null && await MarkDeletedAsync(entity);
        }

        #endregion

        #region [ Any ]

        public virtual bool Any(Guid uid)
        {
            return Any(GetQueryable(uid));
        }

        public virtual async Task<bool> AnyAsync(Guid uid)
        {
            return await AnyAsync(GetQueryable(uid));
        }

        #endregion
    }
}