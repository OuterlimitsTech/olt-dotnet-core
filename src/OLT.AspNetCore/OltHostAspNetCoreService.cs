using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace OLT.Core
{
    public class OltHostAspNetCoreService : IOltHostService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public OltHostAspNetCoreService(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;            
        }

        public string ResolveRelativePath(string filePath)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, filePath.Replace("~/", string.Empty));
        }
               
        public string EnvironmentName => _hostEnvironment.EnvironmentName;
        public string ApplicationName => _hostEnvironment.ApplicationName;
    }
}