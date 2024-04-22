using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{    
    public class OltAssemblyFilter
    {
        /// <summary>
        /// OLT.*, MyApp.*
        /// </summary>
        /// <param name="filters"></param>
        public OltAssemblyFilter(params string[] filters)
        {
            Filters.AddRange(filters);
        }


        /// <summary>
        /// Defaults "Microsoft.*" and "System.*" to prevent Microsoft and System Assemblies from loading into the DI scan
        /// </summary>   
        /// <remarks>
        /// <list type="table">
        /// <item>https://github.com/dotnet/SqlClient/issues/1930</item>
        /// <item>https://github.com/borisdj/EFCore.BulkExtensions/issues/1402</item>
        /// </list>
        /// </remarks>
        public OltAssemblyFilter WithDefaultDIExclusionFilters()
        {
            this.ExcludeFilters = new List<string> { "Microsoft.*", "System.*" };
            return this;
        }

        public List<string> Filters { get; set; } = new List<string>();
        public List<string> ExcludeFilters { get; set; } = new List<string>();

        public virtual IEnumerable<Assembly> FilterAssemblies(IEnumerable<Assembly> assemblies)
        {
            return Filters.Count > 0 ? assemblies.Where(ShouldIncludeAssembly) : assemblies;
        }        

      

        public virtual bool ShouldIncludeAssembly(Assembly assembly)
        {
            return Filters.Exists(filter => MatchesFilter(assembly, filter));
        }

        protected virtual bool MatchesFilter(Assembly assembly, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return false;
            }

            if (assembly.FullName is null)
            {
                return false;
            }

            if (filter.EndsWith("*"))
            {
                var prefix = filter.TrimEnd('*');
                return assembly.FullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                var assemblyName = assembly.FullName.Split(',')[0];
                return string.Equals(assemblyName, filter, StringComparison.OrdinalIgnoreCase);
            }
        }

        public virtual bool ShouldExcludeAssembly(Assembly assembly)
        {
            return ExcludeFilters.Exists(filter => MatchesFilter(assembly, filter));
        }

        public virtual void RemoveAllExclusions(List<Assembly> assemblies)
        {
            ExcludeFilters.ForEach(excludeName => RemoveAll(assemblies, excludeName));
        }        

        public virtual void RemoveAll(List<Assembly> assemblies, string excludeName)
        {
            assemblies.RemoveAll(p => MatchesFilter(p, excludeName));
        }
    }
}
