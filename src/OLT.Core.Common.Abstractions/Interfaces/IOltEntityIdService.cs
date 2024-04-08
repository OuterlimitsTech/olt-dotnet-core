using System.Linq;
using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltEntityIdService<TEntity> : IOltEntityService<TEntity>
        where TEntity : class, IOltEntityId, IOltEntity
    {
        TModel? Get<TModel>(int id, bool includeDeleted = false) where TModel : class, new();

        Task<TModel?> GetAsync<TModel>(int id, bool includeDeleted = false) where TModel : class, new();

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
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(int id, bool includeDeleted = false) where TModel : class, new();

        TModel Update<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new();

        TResponseModel Update<TResponseModel, TSaveModel>(int id, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TModel> UpdateAsync<TModel>(int id, TModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null) where TModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(int id, TSaveModel model, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        bool SoftDelete(int id);
        Task<bool> SoftDeleteAsync(int id);

        bool Any(int id);
        Task<bool> AnyAsync(int id);
    }
}
