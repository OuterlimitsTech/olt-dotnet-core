namespace OLT.Core
{
    public interface IOltEntityIdService<TEntity> : IOltEntityService<TEntity>
        where TEntity : class, IOltEntityId, IOltEntity
    {
        TModel? Get<TModel>(int id, bool includeDeleted = false) where TModel : class, new();

        Task<TModel?> GetAsync<TModel>(int id) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(int id, CancellationToken cancellationToken) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(int id, bool includeDeleted = false) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(int id, bool includeDeleted, CancellationToken cancellationToken) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        TModel GetSafe<TModel>(int id, bool includeDeleted = false) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(int id, CancellationToken cancellationToken = default) where TModel : class, new();

        /// <summary>
        /// Null Safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <param name="includeDeleted"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(int id, bool includeDeleted, CancellationToken cancellationToken = default) where TModel : class, new();

        TModel Update<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new();

        TResponseModel Update<TResponseModel, TSaveModel>(int id, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TModel> UpdateAsync<TModel>(int id, TModel model) where TModel : class, new();
        Task<TModel> UpdateAsync<TModel>(int id, TModel model, CancellationToken cancellationToken) where TModel : class, new();
        Task<TModel> UpdateAsync<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>> include) where TModel : class, new();
        Task<TModel> UpdateAsync<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, CancellationToken cancellationToken = default) where TModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(int id, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(int id, TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(int id, TSaveModel model, CancellationToken cancellationToken)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(int id, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, CancellationToken cancellationToken = default)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        bool SoftDelete(int id);
        Task<bool> SoftDeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken);

        bool Any(int id);
        Task<bool> AnyAsync(int id);
        Task<bool> AnyAsync(int id, CancellationToken cancellationToken);
    }
}
