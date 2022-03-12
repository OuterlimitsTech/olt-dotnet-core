using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public class OltAdapterResolver : OltDisposable, IOltAdapterResolver
    {

        public OltAdapterResolver(IServiceProvider serviceProvider)
        {
            Adapters = serviceProvider.GetServices<IOltAdapter>().ToList();
        }

        protected virtual List<IOltAdapter> Adapters { get; }

        protected virtual string GetAdapterName<TSource, TDestination>()
        {
            return OltAdapterExtensions.BuildAdapterName<TSource, TDestination>();
        }

        #region [ Before & After Maps ]

        public virtual IQueryable<TSource> ApplyBeforeMaps<TSource, TDestination>(IQueryable<TSource> queryable)
        {
            return OltAdapterMapConfigs.ApplyBeforeMaps<TSource, TDestination>(queryable);
        }

        public virtual IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            return OltAdapterMapConfigs.ApplyAfterMaps<TSource, TDestination>(queryable);
        }

        #endregion


        #region [ ProjectTo ]

        public virtual bool CanProjectTo<TSource, TDestination>()
        {
            var name = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(name, false);
            if (adapter is IOltAdapterQueryable<TSource, TDestination>)
            {
                return true;
            }
            return false;
        }

        ///<inheritdoc/>
        public virtual IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source) //, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy = null
        {
            var name = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(name, false);
            return ProjectTo<TSource, TDestination>(source, adapter);
        }


        protected virtual IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltAdapter adapter, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy = null)
        {
            if (adapter is IOltAdapterQueryable<TSource, TDestination> queryableAdapter)
            {
                source = ApplyBeforeMaps<TSource, TDestination>(source);
                if (orderBy != null) orderBy(source);
                return ApplyAfterMaps<TSource, TDestination>(queryableAdapter.Map(source));
            }
            throw new OltAdapterNotFoundException(GetAdapterName<TSource, TDestination>());
        }

        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        public virtual IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams)
            where TSource : class
        {
            var adapter = GetPagedAdapter<TSource, TDestination>(true);
            return adapter.Map(source, pagingParams);
        }

        [Obsolete("Move to Extension with BeforeMap or AfterMap for DefaultOrderBy")]
        public virtual IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy) 
            where TSource : class
        {
            var adapter = GetPagedAdapter<TSource, TDestination>(true);
            return adapter.Map(source, pagingParams, orderBy);
        }

        [Obsolete("Move to BeforeMap or AfterMap")]
        public virtual bool CanMapPaged<TSource, TDestination>()
        {
            return GetAdapter(this.GetAdapterName<TSource, TDestination>(), false) is IOltAdapterPaged<TSource, TDestination>;
        }

        [Obsolete("Move to BeforeMap or AfterMap")]
        protected virtual IOltAdapterPaged<TSource, TDestination> GetPagedAdapter<TSource, TDestination>(bool throwException = true)
            where TSource : class
        {
            var adapterName = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(adapterName, throwException);
            var pagedAdapter = adapter as IOltAdapterPaged<TSource, TDestination>;
            if (pagedAdapter == null && throwException)
            {
                throw new OltAdapterNotFoundException($"{adapterName} Paged");
            }
            return pagedAdapter;
        }

        #endregion

        #region [ Maps ]

        public virtual List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            var adapter = GetAdapter<TSource, TDestination>(false);            
            return adapter == null ? GetAdapter<TDestination, TSource>(true).Map(source.AsEnumerable()).ToList() : adapter.Map(source.AsEnumerable()).ToList();
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var adapter = GetAdapter<TSource, TDestination>(false);
            if (adapter == null)
            {
                GetAdapter<TDestination, TSource>(true).Map(source, destination);
            }
            else
            {
                adapter.Map(source, destination);
            }            
            return destination;
        }

        #endregion

        #region [ Get Adapater Methods ]

        public virtual bool CanMap<TSource, TDestination>()
        {
            return GetAdapter(this.GetAdapterName<TSource, TDestination>(), false) is IOltAdapter<TSource, TDestination> ||
                   GetAdapter(this.GetAdapterName<TDestination, TSource>(), false) is IOltAdapter<TDestination, TSource>;
        }


        public virtual IOltAdapter<TSource, TDestination> GetAdapter<TSource, TDestination>(bool throwException = true)
        {
            var name = this.GetAdapterName<TSource, TDestination>();
            return GetAdapter(name, throwException) as IOltAdapter<TSource, TDestination>;
        }

        protected virtual IOltAdapter GetAdapter(string adapterName, bool throwException = true)
        {
            var adapter = Adapters.FirstOrDefault(p => p.Name == adapterName);
            if (adapter == null && throwException)
            {
                throw new OltAdapterNotFoundException(adapterName);
            }
            return adapter;
        }


        #endregion

    }
}
