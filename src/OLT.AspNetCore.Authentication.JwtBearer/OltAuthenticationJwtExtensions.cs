using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using OLT.AspNetCore.Authentication;

namespace OLT.Core
{
    public static partial class OltAuthenticationJwtExtensions
    {
        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="IOltAuthenticationJwtBearer"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer<TScheme>(this IServiceCollection services, TScheme schemeBuilder)
            where TScheme : IOltAuthenticationJwtBearer
            => services.AddJwtBearer(schemeBuilder, null, null);

        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="IOltAuthenticationJwtBearer"/></param>
        /// <param name="configureOptions"><seealso cref="JwtBearerOptions"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer<TScheme>(this IServiceCollection services, TScheme schemeBuilder, Action<JwtBearerOptions>? configureOptions)
            where TScheme : IOltAuthenticationJwtBearer
            => services.AddJwtBearer(schemeBuilder, configureOptions, null);

        /// <summary>
        /// Adds JWT Bearer Token Authentication
        /// </summary>
        /// <typeparam name="TScheme"></typeparam>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="schemeBuilder"><seealso cref="IOltAuthenticationJwtBearer"/></param>
        /// <param name="authOptionsAction"><seealso cref="AuthenticationOptions"/></param>
        /// <param name="schemeOptions"><seealso cref="JwtBearerOptions"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static AuthenticationBuilder AddJwtBearer<TScheme>(this IServiceCollection services, TScheme schemeBuilder, Action<JwtBearerOptions>? schemeOptions, Action<AuthenticationOptions>? authOptionsAction)
            where TScheme: IOltAuthenticationJwtBearer
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(schemeBuilder);

#if NET8_0_OR_GREATER
            // https://github.com/dotnet/aspnetcore/issues/52075
            //JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
#else
            //But with the Security Token Handler not using the custom RoleClaimType correctly
            //Clearing the DefaultInboundClaimTypeMap before adding the auth fixes the role claims
            //https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1349
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
#endif

            

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