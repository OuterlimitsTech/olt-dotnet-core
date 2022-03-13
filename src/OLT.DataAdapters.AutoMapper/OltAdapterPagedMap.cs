﻿using AutoMapper;
using System;
using System.Linq;

namespace OLT.Core
{
    [Obsolete("Move to BeforeMap or AfterMap")]
    public abstract class OltAdapterPagedMap<TEntity, TModel> : Profile, IOltAdapterPagedMap<TEntity, TModel>
        where TEntity : class
    {

        protected OltAdapterPagedMap() => CreateMaps();

        public string Name => OltAdapterExtensions.BuildAdapterName<TEntity, TModel>();

        public void CreateMaps()
        {            
            var mapping = CreateMap<TEntity, TModel>();
            BuildMap(mapping);
            mapping.BeforeMap(new OltBeforeMapOrderBy<TEntity, TModel>(DefaultOrderBy));
        }

        public abstract void BuildMap(IMappingExpression<TEntity, TModel> mappingExpression);
        public abstract IQueryable<TEntity> DefaultOrderBy(IQueryable<TEntity> queryable);

        #region [ Dispose ]

        /// <summary>
        /// The disposed
        /// </summary>
        protected bool Disposed { get; set; } = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }

        

        #endregion


    }
}