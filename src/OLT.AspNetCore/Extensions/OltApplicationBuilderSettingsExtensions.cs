using Microsoft.AspNetCore.Builder;

namespace OLT.Core
{
    public static partial class OltApplicationBuilderSettingsExtensions
    {
        /// <summary>
        /// Registers middleware <seealso cref="UsePathBaseExtensions"/>  using <seealso cref="OltAspNetHostingOptions.PathBase"/> 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="options"><seealso cref="OltAspNetHostingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UsePathBase<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : OltAspNetHostingOptions
        {
            return string.IsNullOrWhiteSpace(options.PathBase) ?  app : app.UsePathBase(options.PathBase);
        }


        /// <summary>
        /// Registers middleware <seealso cref="DeveloperExceptionPageExtensions"/> using <seealso cref="OltAspNetHostingOptions.ShowExceptionDetails"/> 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="options"><seealso cref="OltAspNetHostingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseDeveloperExceptionPage<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : OltAspNetHostingOptions
        {
            return options.ShowExceptionDetails ? app.UseDeveloperExceptionPage() : app;
        }


        /// <summary>
        /// Registers middleware <seealso cref="HstsBuilderExtensions"/> using <seealso cref="OltAspNetHostingOptions.UseHsts"/> 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="options"><seealso cref="OltAspNetHostingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseHsts<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : OltAspNetHostingOptions
        {
            return options.UseHsts ? app.UseHsts() : app;
        }



        /// <summary>
        /// Registers middleware <seealso cref="HttpsPolicyBuilderExtensions"/> using <seealso cref="OltAspNetHostingOptions.DisableHttpsRedirect"/> 
        /// </summary>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="app"><seealso cref="IApplicationBuilder"/></param>
        /// <param name="options"><seealso cref="OltAspNetHostingOptions"/></param>
        /// <returns><seealso cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseHttpsRedirection<TOptions>(this IApplicationBuilder app, TOptions options)
            where TOptions : OltAspNetHostingOptions
        {
            return options.DisableHttpsRedirect ? app : app.UseHttpsRedirection();
        }

    
    }
}
