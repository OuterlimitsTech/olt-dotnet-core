using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public class OltAdapterResolverAutoMapper : OltAdapterResolver
    {
        public OltAdapterResolverAutoMapper(
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mapper = serviceProvider.GetService<IMapper>();
        }

        protected virtual IMapper Mapper { get; }

        protected virtual bool HasAutoMap<TSource, TDestination>()
        {
            return Mapper.ConfigurationProvider.FindTypeMapFor<TSource, TDestination>() != null;
        }

        protected virtual OltAutoMapperException<TSource, TResult> BuildException<TSource, TResult>(Exception exception)
        {
            if (exception is AutoMapperMappingException autoMapperException)
            {
                return new OltAutoMapperException<TSource, TResult>(autoMapperException);
            }
            return new OltAutoMapperException<TSource, TResult>(exception);
        }

        public override bool CanMap<TSource, TDestination>()
        {
            return CanProjectTo<TSource, TDestination>() || base.CanMap<TSource, TDestination>();
        }

        #region [ ProjectTo Maps ]

        public override bool CanProjectTo<TSource, TDestination>()
        {
            return HasAutoMap<TSource, TDestination>() || base.CanProjectTo<TSource, TDestination>();
        }

        protected override IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltAdapter adapter)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                source = ApplyBeforeMaps<TSource, TDestination>(source);
                return ApplyAfterMaps<TSource, TDestination>(source.ProjectTo<TDestination>(Mapper.ConfigurationProvider));
            }
            return base.ProjectTo<TSource, TDestination>(source, adapter);
        }


        #endregion


        #region [ Paged ]

        [Obsolete("Move to BeforeMap or AfterMap")]
        public override bool CanMapPaged<TSource, TDestination>()
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                return GetPagedAdapterMap<TSource, TDestination>(false) != null;                
            }
            return base.CanMapPaged<TSource, TDestination>();
        }

        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        public override IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                var mapAdapter = GetPagedAdapterMap<TSource, TDestination>(true);
                Func<IQueryable<TSource>, IQueryable<TSource>> orderBy = orderByQueryable => orderByQueryable.OrderBy(null, mapAdapter.DefaultOrderBy);
                return this.ProjectTo<TSource, TDestination>(source, pagingParams, orderBy);
            }

            return base.ProjectTo<TSource, TDestination>(source, pagingParams);
        }

        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        public override IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                try
                {
                    return ApplyAfterMaps<TSource, TDestination>(source.OrderBy(null, orderBy).ProjectTo<TDestination>(Mapper.ConfigurationProvider)).ToPaged(pagingParams);
                }
                catch (Exception ex)
                {
                    throw BuildException<TSource, TDestination>(ex);
                }
            }
            return base.ProjectTo<TSource, TDestination>(source, pagingParams, orderBy);
        }

        protected virtual IOltAdapterPagedMap<TSource, TDestination> GetPagedAdapterMap<TSource, TDestination>(bool throwException)
        {
            var adapterName = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(adapterName, false);
            var mapAdapter = adapter as IOltAdapterPagedMap<TSource, TDestination>;
            if (mapAdapter == null && throwException)
            {
                throw new OltAdapterNotFoundException($"{adapterName} Paged");
            }
            return mapAdapter;

        }

  
        #endregion

        #region [ Maps ]

        public override List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                try
                {
                    return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source.AsEnumerable()).ToList();
                }
                catch (Exception exception)
                {
                    throw BuildException<TSource, TDestination>(exception);
                }
            }

            return base.Map<TSource, TDestination>(source);
        }


        public override TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (HasAutoMap<TSource, TDestination>())
            {
                try
                {                    
                    return Mapper.Map(source, destination);
                }
                catch (Exception exception)
                {
                    throw BuildException<TSource, TDestination>(exception);
                }
            }

            return base.Map(source, destination);
        }

        #endregion

    }
}