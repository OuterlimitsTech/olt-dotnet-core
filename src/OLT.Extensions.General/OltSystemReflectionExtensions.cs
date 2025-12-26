// ReSharper disable once CheckNamespace
namespace System.Reflection
{

    public static class OltSystemReflectionExtensions
    {  

        /// <summary>
        /// Searches Assembly for embedded resource. This method does not require the Fully Qualified Name
        /// </summary>
        /// <remarks>
        /// Example: MyNamespace.SomeDirectory.TheFile.csv -> this.GetType().Assembly.GetEmbeddedResourceStream("TheFile.csv")
        /// </remarks>        
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static Stream GetEmbeddedResourceStreamSafe(this Assembly assembly, string resourceName)
        {
            return GetEmbeddedResourceStream(assembly, resourceName) ?? throw new FileNotFoundException("Cannot find embedded resource.", resourceName);
        }

        /// <summary>
        /// Searches Assembly for embedded resource. This method does not require the Fully Qualified Name
        /// </summary>
        /// <remarks>
        /// Example: MyNamespace.SomeDirectory.TheFile.csv -> this.GetType().Assembly.GetEmbeddedResourceStream("TheFile.csv")
        /// </remarks>
        /// <param name="assembly"><see cref="Assembly"/></param>
        /// <param name="resourceName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>return stream of embedded resource or null if not found</returns>
        public static Stream? GetEmbeddedResourceStream(this Assembly assembly, string resourceName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(resourceName);

            // Get all embedded resources
            string[] arrResources = assembly.GetManifestResourceNames();
            var resourceCompare = resourceName.ToLower();
            var resources = new List<string>();

            resources.AddRange(from resource in arrResources where resource.Contains(resourceCompare, StringComparison.OrdinalIgnoreCase) select resource);

            var name = resources.FirstOrDefault();
            if (resources.Count > 1)
            {
                throw new FileLoadException($"{resources.Count} embedded resources found.", resourceName);
            }
            else if (name != null)
            {
                return assembly.GetManifestResourceStream(name);
            }

            throw new FileNotFoundException("Cannot find embedded resource.", resourceName);
        }

        /// <summary>
        /// Searches Assembly for embedded resource and saves it to file. This method does not require the Fully Qualified Name
        /// </summary>
        /// <remarks>
        /// Example: MyNamespace.SomeDirectory.TheFile.csv -> this.GetType().Assembly.EmbeddedResourceToFile("TheFile.csv")
        /// </remarks>
        /// <param name="assembly"><see cref="Assembly"/></param>
        /// <param name="resourceName"></param>
        /// <param name="fileName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>return stream of embedded resource or null if not found</returns>
        public static void EmbeddedResourceToFile(this Assembly assembly, string resourceName, string fileName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(resourceName);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(fileName);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var stream = GetEmbeddedResourceStream(assembly, resourceName))
            {
                using (FileStream output = new FileStream(fileName, FileMode.Create))
                {
                    stream?.CopyTo(output);
                }
            }

        }


        /// <summary>
        /// return string for embedded resource
        /// </summary>
        /// <remarks>
        /// Example: MyNamespace.SomeDirectory.TheFile.csv -> this.GetType().Assembly.GetEmbeddedResourceString("TheFile.csv")
        /// </remarks>
        /// <param name="assembly"><see cref="Assembly"/></param>
        /// <param name="resourceName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <returns>return stream of embedded resource or null if not found</returns>
        public static string? GetEmbeddedResourceString(this Assembly assembly, string resourceName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(resourceName);

            var resource = GetEmbeddedResourceStream(assembly, resourceName);
            if (resource == null) return null;
            using (StreamReader sr = new StreamReader(resource))
            {
                string data = sr.ReadToEnd();
                sr.Close();
                return data;
            }
        }

        /// <summary>
        /// Scans provided assembly for all objects that implements interfaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns>Returns an instance for all objects</returns>
        public static IEnumerable<T> GetAllImplements<T>(this Assembly assembly)
        {
            return GetAllImplements<T>(new List<Assembly>() { assembly });
        }

        /// <summary>
        /// Scans provided assemblies for all objects that implements interfaces
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns>Returns an instance for all objects</returns>
        public static IEnumerable<T> GetAllImplements<T>(this Assembly[] assemblies)
        {
            return GetAllImplements<T>(assemblies.AsEnumerable());
        }


        /// <summary>
        /// Scans provided assemblies for all objects that implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns>Returns an instance for all objects</returns>
        public static IEnumerable<T> GetAllImplements<T>(this IEnumerable<Assembly> assemblies)
        {
            var result = new List<T>();
            
            foreach (var assembly in assemblies)
            {
                try
                {
                    var loaded = Assembly.Load(assembly.GetName());
                    if (loaded == null) continue;
                    result.AddRange(GetAllImplementsInternal<T>(loaded));
                }
                catch
                {
                    // ignored
                }

            }

            return result;
        }

        private static IEnumerable<T> GetAllImplementsInternal<T>(Assembly assembly)
        {
            foreach (var ti in assembly.DefinedTypes)
            {
                if (ti.ImplementedInterfaces.Contains(typeof(T)) && !ti.IsAbstract && !ti.IsInterface && !ti.IsGenericType && ti.FullName != null)
                {
                    var instance = assembly.CreateInstance(ti.FullName);
                    if (instance is T typedInstance)
                    {
                        yield return typedInstance;
                    }
                }
            }
        }
    }
}