﻿namespace OLT.Core
{
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