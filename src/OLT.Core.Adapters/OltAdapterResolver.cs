//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace OLT.Core
//{
//    public interface IOltAdapterResolver : IOltInjectableSingleton
//    {
//        bool CanProjectTo<TEntity, TDestination>();

//        IQueryable<TSource> Include<TSource, TDestination>(IQueryable<TSource> queryable) where TSource : class;

//        IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source);        

//        IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams) where TSource : class;
//        IOltPaged<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, IOltPagingParams pagingParams, Func<IQueryable<TSource>, IQueryable<TSource>> orderBy) where TSource : class;
                

//        List<TDestination> Map<TSource, TDestination>(List<TSource> source);
//        IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);
//        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

//        Func<IQueryable<TSource>, IQueryable<TSource>> DefaultOrderBy<TSource, TDestination>() where TSource : class;
//    }
//}


//namespace OLT.Core.Adapters
//{
//    public class OltAdapterResolver : OltDisposable, IOltAdapterResolver
//    {
//        public OltAdapterResolver(IServiceProvider serviceProvider)
//        {
//            Adapters = serviceProvider.GetServices<IOltAdapter>().ToList();
//        }

//        protected virtual List<IOltAdapter> Adapters { get; }

//        protected virtual string GetAdapterName<TSource, TDestination>()
//        {
//            return OltAdapterExtensions.BuildAdapterName<TSource, TDestination>();
//        }

//        protected virtual IOltAdapter<TSource, TDestination> GetAdapter<TSource, TDestination>(bool throwException)
//        {
//            return GetAdapter(this.GetAdapterName<TSource, TDestination>(), throwException) as IOltAdapter<TSource, TDestination>;
//        }

//        protected virtual IOltAdapter GetAdapter(string adapterName, bool throwException)
//        {
//            var adapter = Adapters.FirstOrDefault(p => p.Name == adapterName);
//            if (adapter == null && throwException)
//            {
//                throw new OltAdapterNotFoundException(adapterName);
//            }
//            return adapter;
//        }

//        public virtual List<TDestination> Map<TSource, TDestination>(List<TSource> source)
//        {
//            var adapter = GetAdapter<TSource, TDestination>(false);
//            return adapter == null ? 
//                GetAdapter<TDestination, TSource>(true).Map(source.AsEnumerable()).ToList() :  //Reverse the adapter lookup
//                adapter.Map(source.AsEnumerable()).ToList();
//        }

//        public virtual IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
//        {
//            return Map<TSource, TDestination>(source.ToList());
//        }

//        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
//        {
//            var adapter = GetAdapter<TSource, TDestination>(false);
//            if (adapter == null)
//            {
//                GetAdapter<TDestination, TSource>(true).Map(source, destination);
//            }
//            else
//            {
//                adapter.Map(source, destination);
//            }
//            return destination;
//        }

//    }
//}