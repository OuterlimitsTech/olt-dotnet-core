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
        public static IOltApplicationBuilder AddOltIdentity<T, TC>(this IOltApplicationBuilder builder)
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

    }
}
