using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Reflection
{
    public static class OltSystemReflectionExtensions
    {

        #region [ Get Referenced Assemblies ]


        /// <summary>
        /// Gets all referenced assemblies
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<Assembly> GetAllReferencedAssemblies(this Assembly assembly)
        {
            return GetAllReferencedAssemblies(new List<Assembly> { assembly });
        }

        /// <summary>
        /// Gets all referenced assemblies for list of assemblies
        /// </summary>
        /// <param name="assembliesToScan"></param>
        /// <returns></returns>
        public static List<Assembly> GetAllReferencedAssemblies(this Assembly[] assembliesToScan)
        {
            return GetAllReferencedAssemblies(assembliesToScan.ToList());
        }

        /// <summary>
        /// Gets all referenced assemblies for provided list of assemblies
        /// </summary>
        /// <param name="assembliesToScan"></param>
        /// <returns></returns>
        public static List<Assembly> GetAllReferencedAssemblies(this List<Assembly> assembliesToScan)
        {
            var results = new List<Assembly>();
            var referencedAssemblies = new List<Assembly>();

            referencedAssemblies.AddRange(assembliesToScan);

            if (assembliesToScan.Exists(p => p.FullName != Assembly.GetCallingAssembly().FullName))
            {
                referencedAssemblies.Add(Assembly.GetCallingAssembly());
            }

            assembliesToScan.ForEach(assembly =>
            {
                referencedAssemblies.AddRange(assembly.GetReferencedAssemblies().Select(Assembly.Load));
            });

            AppDomain.CurrentDomain
                .GetAssemblies()
                .ToList()
                .ForEach(assembly =>
                {
                    referencedAssemblies.Add(assembly);
                });


            referencedAssemblies
                .GroupBy(g => g.FullName)
                .Select(s => s.Key)
                .OrderBy(o => o)
                .ToList()
                .ForEach(name =>
                {
                    var assembly = results.Find(p => string.Equals(p.FullName, name, StringComparison.OrdinalIgnoreCase));
                    if (assembly == null)
                    {
                        var add = referencedAssemblies.Find(p => p.FullName == name);
                        if (add != null)
                        {
                            results.Add(add);
                        }                        
                    }
                });


            return results;
        }

        #endregion

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
            if (resourceName == null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException($"{resourceName} cannot be null or whitespace");
            }

            // Get all embedded resources
            string[] arrResources = assembly.GetManifestResourceNames();
            var resourceCompare = resourceName.ToLower();
            var resources = new List<string>();

#if NET6_0_OR_GREATER
            resources.AddRange(from resource in arrResources where resource.Contains(resourceCompare, StringComparison.OrdinalIgnoreCase) select resource);
#else
            resources.AddRange(from resource in arrResources where resource.ToLower().Contains(resourceCompare) select resource);
#endif

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
            if (resourceName == null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException($"{resourceName} cannot be null or whitespace");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"{fileName} cannot be null or whitespace");
            }

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
            if (resourceName == null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException($"{resourceName} cannot be null or whitespace");
            }

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
            return GetAllImplements<T>(assemblies.ToList());
        }


        /// <summary>
        /// Scans provided assemblies for all objects that implement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies"></param>
        /// <returns>Returns an instance for all objects</returns>
        public static IEnumerable<T> GetAllImplements<T>(this List<Assembly> assemblies)
        {
            var result = new List<T>();
            
            foreach (var assembly in assemblies)
            {
                try
                {
                    var loaded = Assembly.Load(assembly.GetName());
                    if (loaded == null) continue;
                    result.AddRange(GetAllImplements_Internal<T>(loaded));
                }
                catch
                {
                    // ignored
                }

            }

            return result;
        }

        private static IEnumerable<T> GetAllImplements_Internal<T>(Assembly assembly)
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