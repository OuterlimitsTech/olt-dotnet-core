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

        public List<string> Filters { get; set; } = new List<string>();

        public IEnumerable<Assembly> FilterAssemblies(IEnumerable<Assembly> assemblies)
        {
            return Filters.Any() ? assemblies.Where(ShouldIncludeAssembly) : assemblies;
        }

        public virtual bool ShouldIncludeAssembly(Assembly assembly)
        {
            return Filters.Any(filter => MatchesFilter(assembly, filter));
        }

        protected virtual bool MatchesFilter(Assembly assembly, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return true;
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
    }

}
