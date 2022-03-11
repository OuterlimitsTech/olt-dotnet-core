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

        #region [ Apply After Maps ]

        public virtual IEnumerable<TDestination> ApplyAfterMaps<TSource, TDestination>(IEnumerable<TDestination> list)
        {
            return OltAfterMapConfig.ApplyAfterMaps<TSource, TDestination>(list.AsQueryable());
        }

        public virtual IQueryable<TDestination> ApplyAfterMaps<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            return OltAfterMapConfig.ApplyAfterMaps<TSource, TDestination>(queryable);
        }

        #endregion


        #region [ ProjectTo ]

        public virtual bool CanProjectTo<TEntity, TDestination>()
        {
            var name = GetAdapterName<TEntity, TDestination>();
            var adapter = GetAdapter(name, false);
            if (adapter is IOltAdapterQueryable<TEntity, TDestination>)
            {
                return true;
            }
            return false;
        }

        public virtual IQueryable<TDestination> ProjectTo<TEntity, TDestination>(IQueryable<TEntity> source)
        {
            var name = GetAdapterName<TEntity, TDestination>();
            var adapter = GetAdapter(name, false);
            return ProjectTo<TEntity, TDestination>(source, adapter);
        }

        protected virtual IQueryable<TDestination> ProjectTo<TEntity, TDestination>(IQueryable<TEntity> source, IOltAdapter adapter)
        {
            if (adapter is IOltAdapterQueryable<TEntity, TDestination> queryableAdapter)
            {
                return ApplyAfterMaps<TEntity, TDestination>(queryableAdapter.Map(source));
            }

            throw new OltAdapterNotFoundException(GetAdapterName<TEntity, TDestination>());
        }

        #endregion

        #region [ Maps ]

        //public virtual IEnumerable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> source)
        //{
        //    var name = GetAdapterName<TSource, TDestination>();
        //    var adapter = GetAdapter(name, true);
        //    return Map<TSource, TDestination>(source, adapter);
        //}

        //protected virtual IQueryable<TDestination> Map<TSource, TDestination>(IQueryable<TSource> source, IOltAdapter adapter)
        //{
        //    if (adapter is IOltAdapterQueryable<TSource, TDestination> queryableAdapter)
        //    {
        //        return ProjectTo<TSource, TDestination>(source, queryableAdapter);
        //    }

        //    // ReSharper disable once PossibleNullReferenceException
        //    var list = (adapter as IOltAdapter<TSource, TDestination>).Map(source).AsQueryable();
        //    return ApplyAfterMaps<TSource, TDestination>(list);
        //}

        public virtual List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            var adapter = GetAdapter<TSource, TDestination>(false);
            return adapter == null ? GetAdapter<TDestination, TSource>(true).Map(source.AsEnumerable()).ToList() : adapter.Map(source.AsEnumerable()).ToList();
        }

        //public virtual IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        //{
        //    return Map<TSource, TDestination>(source.ToList());
        //}

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

    }
}
