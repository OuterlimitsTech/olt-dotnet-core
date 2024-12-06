using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using OLT.AspNetCore.Authentication;
using System;

namespace OLT.Core
{
    public static partial class OltAuthenticationJwtExtensions
    {
        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="OltAuthenticationJwtBearer"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer(this IServiceCollection services, OltAuthenticationJwtBearer schemeBuilder)
            => services.AddJwtBearer(schemeBuilder, null, null);

        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="OltAuthenticationJwtBearer"/></param>
        /// <param name="configureOptions"><seealso cref="JwtBearerOptions"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer(this IServiceCollection services, OltAuthenticationJwtBearer schemeBuilder, Action<JwtBearerOptions>? configureOptions)
            => services.AddJwtBearer(schemeBuilder, configureOptions, null);

        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="OltAuthenticationJwtBearer"/></param>
        /// <param name="authOptionsAction"><seealso cref="AuthenticationOptions"/></param>
        /// <param name="schemeOptions"><seealso cref="JwtBearerOptions"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer(this IServiceCollection services, OltAuthenticationJwtBearer schemeBuilder, Action<JwtBearerOptions>? schemeOptions, Action<AuthenticationOptions>? authOptionsAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(schemeBuilder);

            // https://github.com/dotnet/aspnetcore/issues/52075
            //JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();



            var builder = services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptionsAction?.Invoke(opt);
                });

            return schemeBuilder.AddScheme(builder, schemeOptions);            
        }
    }

   
}