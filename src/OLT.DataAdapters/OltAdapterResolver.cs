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

        public IQueryable<TSource> ApplyDefaultOrderBy<TSource, TDestination>(IQueryable<TSource> queryable)
        {
            var pagedAdapter = GetPagedAdapter<TSource, TDestination>();
            if (pagedAdapter != null)
            {
                return pagedAdapter.DefaultOrderBy(queryable);
            }
            return ApplyBeforeMaps<TSource, TDestination>(queryable);
        }


        #region [ Before & After Maps ]
                
        protected virtual IQueryable<TSource> ApplyBeforeMaps<TSource, TDestination>(IQueryable<TSource> queryable)
        {
            return OltAdapterMapConfigs.BeforeMap.Apply<TSource, TDestination>(queryable);
        }

        protected virtual IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            return OltAdapterMapConfigs.AfterMap.Apply<TSource, TDestination>(queryable);
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

        public virtual IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, Action<OltAdapterActionConfig>? configAction = null)
        {
            var name = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(name, false);
            return ProjectTo<TSource, TDestination>(source, configAction, adapter);
        }

        protected virtual IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, Action<OltAdapterActionConfig>? configAction, IOltAdapter? adapter)
        {
            if (adapter is IOltAdapterQueryable<TSource, TDestination> queryableAdapter)
            {
                var config = new OltAdapterActionConfig();
                configAction?.Invoke(config);

                source = config.DisableBeforeMap ? source : ApplyBeforeMaps<TSource, TDestination>(source);
                var mapped = queryableAdapter.Map(source);
                return config.DisableAfterMap ? mapped : ApplyAfterMaps<TSource, TDestination>(mapped);
            }

            throw new OltAdapterNotFoundException(GetAdapterName<TSource, TDestination>());
        }

        #endregion

        #region [ Paged ]

        protected virtual IOltAdapterPaged<TSource, TDestination>? GetPagedAdapter<TSource, TDestination>()
        {
            var adapterName = GetAdapterName<TSource, TDestination>();
            var adapter = GetAdapter(adapterName, false);
            var pagedAdapter = adapter as IOltAdapterPaged<TSource, TDestination>;
            return pagedAdapter;
        }

        #endregion

        #region [ Maps ]

        public virtual List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            var adapter = GetAdapter<TSource, TDestination>(false);
            if (adapter != null)
            {
                return adapter.Map(source.AsEnumerable()).ToList();
            }
            var flipped = GetAdapter<TDestination, TSource>(false);
            if (flipped == null) throw new OltAdapterNotFoundException<TSource, TDestination>();
            return flipped.Map(source.AsEnumerable()).ToList();
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var adapter = GetAdapter<TSource, TDestination>(false);
            if (adapter == null)
            {
                var flipped = GetAdapter<TDestination, TSource>(false);
                if (flipped == null) throw new OltAdapterNotFoundException<TSource, TDestination>();
                flipped.Map(source, destination);
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

        public virtual IOltAdapter<TSource, TDestination>? GetAdapter<TSource, TDestination>(bool throwException = true)
        {
            var name = this.GetAdapterName<TSource, TDestination>();
            return GetAdapter(name, throwException) as IOltAdapter<TSource, TDestination>;
        }

        protected virtual IOltAdapter? GetAdapter(string adapterName, bool throwException = true)
        {
            var adapter = Adapters.Find(p => p.Name == adapterName);
            if (adapter == null && throwException)
            {
                throw new OltAdapterNotFoundException(adapterName);
            }
            return adapter;
        }




        #endregion

    }
}
