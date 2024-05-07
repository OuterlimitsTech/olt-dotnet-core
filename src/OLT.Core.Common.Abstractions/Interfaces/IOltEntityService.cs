﻿using System.Linq.Expressions;

namespace OLT.Core
{
    public interface IOltEntityService<TEntity> : IOltCoreService
        where TEntity : class, IOltEntity
    {
        IEnumerable<TModel> GetAll<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();
        IEnumerable<TModel> GetAll<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();
        IEnumerable<TModel> GetAll<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();
        IEnumerable<TModel> GetAll<TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy) where TModel : class, new();
        IEnumerable<TModel> GetAll<TModel>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(IOltSearcher<TEntity> searcher, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy) where TModel : class, new();
        Task<IEnumerable<TModel>> GetAllAsync<TModel>(bool includeDeleted, Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();

        TModel? Get<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();
        TModel? Get<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();
        TModel? Get<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();

        Task<TModel?> GetAsync<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="searcher"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        TModel GetSafe<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        TModel GetSafe<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="includeDeleted"></param>
        /// <param name="searchers"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        TModel GetSafe<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="searcher"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(IOltSearcher<TEntity> searcher) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(Expression<Func<TEntity, bool>> predicate) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="includeDeleted"></param>
        /// <param name="searchers"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(bool includeDeleted, params IOltSearcher<TEntity>[] searchers) where TModel : class, new();


        IOltPaged<TModel> GetPaged<TModel>(IOltSearcher<TEntity> searcher, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null) where TModel : class, new();

        Task<IOltPaged<TModel>> GetPagedAsync<TModel>(IOltSearcher<TEntity> searcher, IOltPagingParams pagingParams, Func<IQueryable<TEntity>, IQueryable<TEntity>>? orderBy = null)  where TModel : class, new();

        TModel Add<TModel>(TModel model) where TModel : class, new();
        List<TModel> Add<TModel>(List<TModel> list) where TModel : class, new();
        IEnumerable<TModel> Add<TModel>(IEnumerable<TModel> collection) where TModel : class, new();

        TResponseModel Add<TResponseModel, TSaveModel>(TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        IEnumerable<TResponseModel> Add<TResponseModel, TSaveModel>(IEnumerable<TSaveModel> list)
            where TSaveModel : class, new()
            where TResponseModel : class, new();

        Task<TModel> AddAsync<TModel>(TModel model) where TModel : class, new();
        Task<List<TModel>> AddAsync<TModel>(List<TModel> list) where TModel : class, new();
        Task<IEnumerable<TModel>> AddAsync<TModel>(IEnumerable<TModel> collection) where TModel : class, new();

        Task<TResponseModel> AddAsync<TResponseModel, TSaveModel>(TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<List<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(List<TSaveModel> list)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<IEnumerable<TResponseModel>> AddAsync<TResponseModel, TSaveModel>(IEnumerable<TSaveModel> collection)
            where TSaveModel : class, new()
            where TResponseModel : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searcher"></param>
        /// <param name="model"></param>
        /// <param name="include"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        TModel Update<TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new();
        TResponseModel Update<TResponseModel, TSaveModel>(IOltSearcher<TEntity> searcher, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TSaveModel : class, new()
            where TResponseModel : class, new();

        Task<TModel> UpdateAsync<TModel>(IOltSearcher<TEntity> searcher, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new();
        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(IOltSearcher<TEntity> searcher, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TSaveModel : class, new()
            where TResponseModel : class, new();


        bool SoftDelete(IOltSearcher<TEntity> searcher);
        Task<bool> SoftDeleteAsync(IOltSearcher<TEntity> searcher);

        int Count(IOltSearcher<TEntity> searcher);
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(IOltSearcher<TEntity> searcher);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        bool Any(IOltSearcher<TEntity> searcher);
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(IOltSearcher<TEntity> searcher);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
