using System;

namespace OLT.Core
{
    [Obsolete("Move to Nuget Package OLT.Utility.AssemblyScanner")]
    public class OltInjectionAssemblyFilter : OltAssemblyFilter
    {
        /// <summary>
        /// OLT.*, MyApp.*, and includes exclustions <seealso cref="OltAssemblyFilter.WithDefaultDIExclusionFilters"/>
        /// </summary>
        /// <param name="filters"></param>
        public OltInjectionAssemblyFilter(params string[] filters) : base(filters)
        {
            base.WithDefaultDIExclusionFilters();
        }
    }
}   