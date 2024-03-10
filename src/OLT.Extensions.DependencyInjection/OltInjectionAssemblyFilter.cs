using System.Collections.Generic;

namespace OLT.Core
{
    public class OltInjectionAssemblyFilter : OltAssemblyFilter
    {
        /// <summary>
        /// OLT.*, MyApp.*
        /// </summary>
        /// <param name="filters"></param>
        public OltInjectionAssemblyFilter(params string[] filters) : base(filters)
        {
            
        }        

        private List<string> _exclude = new List<string>
        {
            "Microsoft.*",
            "System.*"
        };

        // https://github.com/dotnet/SqlClient/issues/1930
        // https://github.com/borisdj/EFCore.BulkExtensions/issues/1402
        /// <summary>
        /// Defaults "Microsoft.*" and "System.*" to prevent Microsoft and System Assemblies from loading into the DI scan
        /// </summary>
        public List<string> ExcludeFilter => _exclude;
    }
}