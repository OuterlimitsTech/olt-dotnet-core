using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    public static class OltBuilderAbstractionExtensions
    {
        /// <summary>
        /// Adds <seealso cref="IOltIdentity"/>, <seealso cref="IOltDbAuditUser"/>, <typeparamref name="T"/>, and <typeparamref name="TC"/> to 
        /// <seealso cref="ServiceCollectionServiceExtensions.AddScoped(IServiceCollection, Type)"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TC"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IOltApplicationHostBuilder AddOltIdentity<T, TC>(this IOltApplicationHostBuilder builder)
            where T : IOltIdentity
            where TC : class, IOltIdentity
        {

            builder.Services
                .AddScoped(typeof(TC))
                .AddScoped(typeof(T), x => x.GetRequiredService<TC>())
                .AddScoped<IOltIdentity>(x => x.GetRequiredService<TC>())
                .AddScoped<IOltDbAuditUser>(x => x.GetRequiredService<TC>())
                ;

            return builder;
        }


        /// <summary>
        /// Adds development configuration settings to the application builder if <seealso cref="System.Diagnostics.Debugger.IsAttached"/>
        /// </summary>
        /// <remarks>
        /// This should be wrapped with 
        /// </remarks>        
        /// <typeparam name="T">The type used to identify the user secrets configuration.</typeparam>
        /// <param name="builder">The application builder to add the configuration to.</param>
        /// <param name="debuggerAttached">A boolean indicating if the debugger is attached. <seealso cref="System.Diagnostics.Debugger.IsAttached"/></param>
        /// <returns>The updated application builder.</returns>
        public static IOltApplicationHostBuilder AddDevelopmentConfig<T>(this IOltApplicationHostBuilder builder, bool debuggerAttached) where T : class
        {
            if (debuggerAttached)
            {
                builder.Configuration
                    .AddJsonFile("appsettings.Development.json", true, true)
                    .AddUserSecrets<T>();
            }

            return builder;
        }
    }
}
