using System;
using System.Linq;

namespace OLT.Core
{
    public class OltAdapterActionConfig
    {
        /// <summary>
        /// Disables BeforeMap of <seealso cref="IOltAdapterResolver.ProjectTo{TSource, TDestination}(IQueryable{TSource}, Action{OltAdapterActionConfig})"/>
        /// </summary>
        public bool DisableBeforeMap {  get; set; }

        /// <summary>
        /// Disables AfterMap of <seealso cref="IOltAdapterResolver.ProjectTo{TSource, TDestination}(IQueryable{TSource}, Action{OltAdapterActionConfig})"/>
        /// </summary>
        public bool DisableAfterMap { get; set; }
    }
}
