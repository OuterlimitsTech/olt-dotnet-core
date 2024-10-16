﻿using System;
using System.Threading.Tasks;

namespace OLT.Core
{
    public interface IOltEntityUniqueIdService<TEntity> : IOltEntityService<TEntity>
        where TEntity : class, IOltEntityUniqueId, IOltEntity
    {
        TModel? Get<TModel>(Guid uid) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(Guid uid) where TModel : class, new();
        Task<TModel?> GetAsync<TModel>(Guid uid, CancellationToken cancellationToken) where TModel : class, new();

        /// <summary>
        /// Null safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        TModel GetSafe<TModel>(Guid uid) where TModel : class, new();

        /// <summary>
        /// Null safe Get
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="uid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OltRecordNotFoundException"></exception>
        Task<TModel> GetSafeAsync<TModel>(Guid uid, CancellationToken cancellationToken = default) where TModel : class, new();

        TModel Update<TModel>(Guid uid, TModel model) where TModel : class, new();

        TResponseModel Update<TResponseModel, TSaveModel>(Guid uid, TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TModel> UpdateAsync<TModel>(Guid uid, TModel model) where TModel : class, new();
        Task<TModel> UpdateAsync<TModel>(Guid uid, TModel model, CancellationToken cancellationToken) where TModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(Guid uid, TSaveModel model)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        Task<TResponseModel> UpdateAsync<TResponseModel, TSaveModel>(Guid uid, TSaveModel model, CancellationToken cancellationToken)
            where TResponseModel : class, new()
            where TSaveModel : class, new();

        bool SoftDelete(Guid uid);
        Task<bool> SoftDeleteAsync(Guid uid);
        Task<bool> SoftDeleteAsync(Guid uid, CancellationToken cancellationToken);

        bool Any(Guid uid);
        Task<bool> AnyAsync(Guid uid);
        Task<bool> AnyAsync(Guid uid, CancellationToken cancellationToken);
    }
}
