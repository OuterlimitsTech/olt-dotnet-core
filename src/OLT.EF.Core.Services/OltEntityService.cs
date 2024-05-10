using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OLT.Core
{
    public abstract class OltEntityService<TContext, TEntity> : OltContextService<TContext>, IOltEntityService<TEntity>
     where TEntity : class, IOltEntity, new()
     where TContext : IOltDbContext
    {

        protected OltEntityService(
            IOltServiceManager serviceManager,
            TContext context) : base(serviceManager, context)
        {
        }

        /// <summary>
        /// Used for Adding new Entities
        /// </summary>
        protected virtual DbSet<TEntity> Repository => Context.Set<TEntity>();

        #region [ Queryable Methods ]

        /// <summary>
        /// Initializes Queryable for Methods.  Override this for things like Inlude.
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> InitializeQueryable(bool includeDeleted)
        {
            return InitializeQueryable<TEntity>(includeDeleted);
        }

        /// <summary>
        /// Returns Queryable using <seealso cref="InitializeQueryable"/> for non-deleted records
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetQueryable() => InitializeQueryable(false);

        #endregion

        #region [ Get ]

        protected virtual TModel? Get<TModel>(IQueryable<TEntity> queryable) where TModel : class, new()
        {
            return Get<TEntity, TModel>(queryable);
        }

        public virtual TModel? Get<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new()
            => this.Get<TModel>(GetQueryable(searcher));

        public virtual TModel? Get<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.Get<TModel>(GetQueryable(includeDeleted, searchers));

        public virtual TModel? Get<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new()
        {
            var queryable = this.GetQueryable().Where(predicate);
            return Get<TModel>(queryable);
        }

        #endregion

        #region [ Get Async ]

        protected virtual Task<TModel?> GetAsync<TModel>(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default) where TModel : class, new()
            => GetAsync<TEntity, TModel>(queryable, cancellationToken);

        #region [Group]

        public virtual Task<TModel?> GetAsync<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new()
            => this.GetAsync<TModel>(GetQueryable(searcher), CancellationToken.None);

        public virtual Task<TModel?> GetAsync<TModel>(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken) where TModel : class, new()
            => this.GetAsync<TModel>(GetQueryable(searcher), cancellationToken);

        #endregion

        #region [Group ]

        public virtual Task<TModel?> GetAsync<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => GetAsync<TModel>(includeDeleted, searchers);

        public virtual Task<TModel?> GetAsync<TModel>(bool includeDeleted, CancellationToken cancellationToken, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAsync<TModel>(GetQueryable(includeDeleted, searchers), cancellationToken);

        #endregion

        #region [Group]

        public virtual Task<TModel?> GetAsync<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new()
            => GetAsync<TModel>(predicate, CancellationToken.None);

        public async Task<TModel?> GetAsync<TModel>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TModel : class, new()
        {
            var queryable = this.GetQueryable().Where(predicate);
            return await GetAsync<TModel>(queryable, cancellationToken);
        }

        #endregion


        #endregion

        #region [ Get Safe ]

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="searcher"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual TModel GetSafe<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new()
            => this.Get<TModel>(searcher) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual TModel GetSafe<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new()
            => this.Get<TModel>(predicate) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="includeDeleted"></param>
        /// <param name="searchers"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual TModel GetSafe<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.Get<TModel>(includeDeleted, searchers) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        #endregion

        #region [ Get Safe Async ]

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="searcher"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual async Task<TModel> GetSafeAsync<TModel>(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken = default) where TModel : class, new()
            => await this.GetAsync<TModel>(searcher) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual async Task<TModel> GetSafeAsync<TModel>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TModel : class, new()
            => await this.GetAsync<TModel>(predicate, cancellationToken) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="includeDeleted"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="searchers"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        public virtual async Task<TModel> GetSafeAsync<TModel>(bool includeDeleted, CancellationToken cancellationToken, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => await this.GetAsync<TModel>(includeDeleted, cancellationToken, searchers) ?? throw new OltRecordNotFoundException($"{typeof(TEntity).Name} not found");

        #endregion

        #region [ Get All ]

        protected virtual IEnumerable<TModel> GetAll<TModel>(IQueryable<TEntity> queryable) where TModel : class, new()
            => GetAll<TEntity, TModel>(queryable);

        public virtual IEnumerable<TModel> GetAll<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new()
            => this.GetAll<TModel>(GetQueryable(searcher));

        public virtual IEnumerable<TModel> GetAll<TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy) where TModel : class, new()
            => this.GetAll<TModel>(GetQueryable(searcher, orderBy));

        public virtual IEnumerable<TModel> GetAll<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAll<TModel>(GetQueryable(includeDeleted, searchers));

        public virtual IEnumerable<TModel> GetAll<TModel>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAll<TModel>(GetQueryable(includeDeleted, orderBy, searchers));

        public virtual IEnumerable<TModel> GetAll<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new()
        {
            var queryable = this.GetQueryable().Where(predicate);
            return GetAll<TModel>(queryable);
        }

        #endregion

        #region [ Get All Async ]

        protected virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(IQueryable<TEntity> queryable, CancellationToken cancellationToken = default) where TModel : class, new()
            => GetAllAsync<TEntity, TModel>(queryable, cancellationToken);

        #region [Group]

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy) where TModel : class, new()
            => this.GetAllAsync<TModel>(searcher, orderBy, CancellationToken.None);

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, CancellationToken cancellationToken) where TModel : class, new()
            => this.GetAllAsync<TModel>(GetQueryable(searcher, orderBy), cancellationToken);

        #endregion

        #region [Group]

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new()
            => this.GetAllAsync<TModel>(searcher, CancellationToken.None);

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken) where TModel : class, new()
            => this.GetAllAsync<TModel>(GetQueryable(searcher), cancellationToken);

        #endregion

        #region [Group]

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
           => GetAllAsync<TModel>(includeDeleted, CancellationToken.None, searchers);

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, CancellationToken cancellationToken, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAllAsync<TModel>(GetQueryable(includeDeleted, searchers), cancellationToken);

        #endregion

        #region [Group]

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAllAsync<TModel>(includeDeleted, orderBy, searchers);

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, CancellationToken cancellationToken, params IOltSearcher<TEntity>[] searchers) where TModel : class, new()
            => this.GetAllAsync<TModel>(GetQueryable(includeDeleted, orderBy, searchers), cancellationToken);

        #endregion

        #region [Group]

        public virtual Task<IEnumerable<TModel>> GetAllAsync<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new()
            => this.GetAllAsync<TModel>(predicate, CancellationToken.None);

        public virtual async Task<IEnumerable<TModel>> GetAllAsync<TModel>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TModel : class, new()
        {
            var queryable = this.GetQueryable().Where(predicate);
            return await GetAllAsync<TModel>(queryable, cancellationToken);
        }
        #endregion

        #endregion

        #region [ Get Paged ]

        public virtual IOltPaged<TModel> GetPaged<TModel>(IOltSearcher<TEntity> searcher, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null)
            where TModel : class, new()
        {
            return this.GetPaged<TModel>(GetQueryable(searcher), pagingParams, orderBy);
        }

        protected virtual IOltPaged<TModel> GetPaged<TModel>(IQueryable<TEntity> queryable, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null)
            where TModel : class, new()
        {
            return MapPaged<TEntity, TModel>(queryable, pagingParams, orderBy);
        }

        #endregion

        #region [ Get Paged Async ]

        public virtual Task<IOltPaged<TModel>> GetPagedAsync<TModel>(IOltSearcher<TEntity> searcher, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null) where TModel : class, new()
            => GetPagedAsync<TModel>(searcher, pagingParams, orderBy, CancellationToken.None);

        public virtual Task<IOltPaged<TModel>> GetPagedAsync<TModel>(IOltSearcher<TEntity> searcher, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            return GetPagedAsync<TModel>(GetQueryable(searcher), pagingParams, orderBy, cancellationToken);
        }

        protected virtual Task<IOltPaged<TModel>> GetPagedAsync<TModel>(IQueryable<TEntity> queryable, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null, CancellationToken cancellationToken = default)
            where TModel : class, new()
        {
            return MapPagedAsync<TEntity, TModel>(queryable, pagingParams, orderBy, cancellationToken);
        }

        #endregion

        #region [ Add List ]

        protected virtual List<TEntity> AddFromList<TModel>(List<TModel> list) where TModel : class, new()
        {
            var entities = new List<TEntity>();
            list.ForEach(item =>
            {
                var entity = new TEntity();
                ServiceManager.AdapterResolver.Map(item, entity);
                entities.Add(entity);
                Repository.Add(entity);
            });
            SaveChanges();
            return entities;
        }

        protected virtual async Task<List<TEntity>> AddFromListAsync<TModel>(List<TModel> list, CancellationToken cancellationToken = default) where TModel : class, new()
        {
            var entities = new List<TEntity>();
            foreach(var item in list)
            {
                var entity = new TEntity();
                ServiceManager.AdapterResolver.Map(item, entity);
                entities.Add(entity);
                await Repository.AddAsync(entity, cancellationToken);
            }
            await SaveChangesAsync(cancellationToken);
            return entities;
        }

        #endregion

        #region [ Build Result List ]

        /// <summary>
        /// Builds result from Adding a list of entities
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual List<TModel> BuildResultList<TModel>(List<TEntity> entities) where TModel : class, new()
        {
            var returnList = new List<TModel>();
            entities.ForEach(entity =>
            {
                var response = new TModel();
                ServiceManager.AdapterResolver.Map(entity, response);
                returnList.Add(response);
            });
            return returnList;
        }

        #endregion

        #region [ Add ]

        public virtual TModel Add<TModel>(TModel model)
                   where TModel : class, new()
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            Repository.Add(entity);
            SaveChanges();
            var response = new TModel();
            ServiceManager.AdapterResolver.Map(entity, response);
            return response;
        }


        public virtual List<TModel> Add<TModel>(List<TModel> list) where TModel : class, new()
        {
            return BuildResultList<TModel>(AddFromList(list));
        }

        public virtual IEnumerable<TModel> Add<TModel>(IEnumerable<TModel> collection) where TModel : class, new()
        {
            return Add(collection.ToList());
        }

        public virtual TResponseModel Add<TResponseModel, TSaveModel>(TSaveModel model)
            where TSaveModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            Repository.Add(entity);
            SaveChanges();
            var response = new TResponseModel();
            ServiceManager.AdapterResolver.Map(entity, response);
            return response;
        }

        public virtual IEnumerable<TResponseModel> Add<TResponseModel, TSaveModel>(IEnumerable<TSaveModel> list)
            where TSaveModel : class, new()
            where TResponseModel : class, new()
        {
            return BuildResultList<TResponseModel>(AddFromList(list.ToList()));
        }

        #endregion

        #region [ Add Async ]


        public virtual Task<TModel> AddAsync<TModel>(TModel model) where TModel : class, new()
        {
            return AddAsync<TModel>(model, CancellationToken.None);
        }     

        public virtual async Task<TModel> AddAsync<TModel>(TModel model, CancellationToken cancellationToken) where TModel : class, new()
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity);
            await SaveChangesAsync();
            var response = new TModel();
            ServiceManager.AdapterResolver.Map(entity, response);
            return response;
        }

        public virtual Task<List<TModel>> AddAsync<TModel>(List<TModel> list) where TModel : class, new()
        {
            return AddAsync<TModel>(list, CancellationToken.None);
        }

        public virtual async Task<List<TModel>> AddAsync<TModel>(List<TModel> list, CancellationToken cancellationToken) where TModel : class, new()
        {
            return BuildResultList<TModel>(await AddFromListAsync(list, cancellationToken));
        }

        public virtual Task<IEnumerable<TModel>> AddAsync<TModel>(IEnumerable<TModel> collection) where TModel : class, new()
        {
            return AddAsync<TModel>(collection, CancellationToken.None);
        }

        public virtual async Task<IEnumerable<TModel>> AddAsync<TModel>(IEnumerable<TModel> collection, CancellationToken cancellationToken) where TModel : class, new()
        {
            return await AddAsync(collection.ToList(), cancellationToken);
        }

        public virtual Task<TResponseModel> AddAsync<TResponseModel, TSaveModel>(TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new()
        {
            return AddAsync<TResponseModel, TSaveModel>(model, CancellationToken.None);
        }

        public virtual async Task<TResponseModel> AddAsync<TResponseModel, TSaveModel>(TSaveModel model, CancellationToken cancellationToken )
            where TSaveModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = new TEntity();
            ServiceManager.AdapterResolver.Map(model, entity);
            await Repository.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
            var response = new TResponseModel();
            ServiceManager.AdapterResolver.Map(entity, response);
            return response;
        }

        public virtual Task<List<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(List<TSaveModel> list)
            where TResponseModel : class, new()
            where TSaveModel : class, new()
        {
            return AddAsync<TResponseModel, TSaveModel>(list, CancellationToken.None);
        }

        public virtual async Task<List<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(List<TSaveModel> list, CancellationToken cancellationToken) 
            where TResponseModel : class, new()
            where TSaveModel : class, new()
        {
            return BuildResultList<TResponseModel>(await AddFromListAsync(list.ToList(), cancellationToken));
        }

        public virtual Task<IEnumerable<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(IEnumerable<TSaveModel> collection)
            where TResponseModel : class, new()
            where TSaveModel : class, new()
        {
            return AddAsync<TResponseModel, TSaveModel>(collection, CancellationToken.None);
        }

        public virtual async Task<IEnumerable<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(IEnumerable<TSaveModel> collection, CancellationToken cancellationToken) where TResponseModel : class, new() where TSaveModel : class, new()
        {
            return await this.AddAsync<TResponseModel, TSaveModel>(collection.ToList(), cancellationToken);
        }

       




        #endregion

        #region [ Update ]        

        public virtual TModel Update<TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new()
        {
            var entity = GetQueryable(searcher).FirstOrDefault();
            ServiceManager.AdapterResolver.Map(model, entity);
            SaveChanges();
            return GetSafe<TModel>(searcher);
        }

        public virtual TResponseModel Update<TResponseModel, TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = GetQueryable(searcher).FirstOrDefault();
            ServiceManager.AdapterResolver.Map(model, entity);
            SaveChanges();
            return GetSafe<TResponseModel>(searcher);
        }

        #endregion

        #region [ Update Async ]

        public virtual Task<TModel> UpdateAsync<TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new()
        {
            return UpdateAsync<TModel>(searcher, model, include, CancellationToken.None);
        }


        public virtual async Task<TModel> UpdateAsync<TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default) where TModel : class, new()
        {
            var entity = await GetQueryable(searcher).FirstOrDefaultAsync(cancellationToken);
            ServiceManager.AdapterResolver.Map(model, entity);
            await SaveChangesAsync(cancellationToken);
            return await GetSafeAsync<TModel>(searcher, cancellationToken);
        }

        public virtual Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(IOltSearcher<TEntity> searcher, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResponseModel : class, new()
            where TSaveModel : class, new()
        {
            return UpdateAsync<TResponseModel, TSaveModel>(searcher, model, include, CancellationToken.None);
        }


        public virtual async Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(IOltSearcher<TEntity> searcher, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, CancellationToken cancellationToken = default)
            where TSaveModel : class, new()
            where TResponseModel : class, new()
        {
            var entity = await GetQueryable(searcher).FirstOrDefaultAsync(cancellationToken);
            ServiceManager.AdapterResolver.Map(model, entity);
            await SaveChangesAsync(cancellationToken);
            return await GetSafeAsync<TResponseModel>(searcher, cancellationToken);
        }

        #endregion

        #region [ Soft Delete ]

        public virtual bool SoftDelete(IOltSearcher<TEntity> searcher)
        {
            var entity = GetQueryable(searcher).FirstOrDefault();
            return entity != null && MarkDeleted(entity);
        }

        public virtual Task<bool> SoftDeleteAsync(IOltSearcher<TEntity> searcher) => SoftDeleteAsync(searcher, CancellationToken.None);

        public virtual async Task<bool> SoftDeleteAsync(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken)
        {
            var entity = await GetQueryable(searcher).FirstOrDefaultAsync(cancellationToken);
            return entity != null && await MarkDeletedAsync(entity, cancellationToken);
        }

        #endregion

        #region [ Count ]

        public virtual int Count(IOltSearcher<TEntity> searcher)
        {
            return Count(GetQueryable(searcher));
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return Count(this.GetQueryable().Where(predicate));
        }

        public virtual Task<int> CountAsync(IOltSearcher<TEntity> searcher) => CountAsync(searcher, CancellationToken.None);

        public virtual async Task<int> CountAsync(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken)
        {
            return await CountAsync(GetQueryable(searcher), cancellationToken);
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) => CountAsync(predicate, CancellationToken.None);

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await CountAsync(this.GetQueryable().Where(predicate), cancellationToken);
        }

        #endregion

        #region [ Any ]

        public virtual bool Any(IOltSearcher<TEntity> searcher)
        {
            return Any(GetQueryable(searcher));
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return Any(this.GetQueryable().Where(predicate));
        }

        public virtual Task<bool> AnyAsync(IOltSearcher<TEntity> searcher) => AnyAsync(searcher, CancellationToken.None);

        public virtual async Task<bool> AnyAsync(IOltSearcher<TEntity> searcher, CancellationToken cancellationToken)
        {
            return await AnyAsync(GetQueryable(searcher), cancellationToken);
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => AnyAsync(predicate, CancellationToken.None);

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await AnyAsync(this.GetQueryable().Where(predicate), cancellationToken);
        }

        #endregion
       

    }
}
