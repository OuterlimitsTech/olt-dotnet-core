using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.AspNetCore.Authentication
{
    [Obsolete("Removing in 8.x")]
    public abstract class OltAuthenticationBuilder : IOltAuthenticationBuilder
    {
        public abstract string Scheme { get; }

        /// <summary>
        /// Adds Authentication setting <seealso cref="AuthenticationOptions.DefaultAuthenticateScheme"/> and <seealso cref="AuthenticationOptions.DefaultChallengeScheme"/> to <seealso cref="Scheme"/>
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Removing in 8.x")]
        public virtual AuthenticationBuilder AddAuthentication(IServiceCollection services)
        {
            return AddAuthentication(services, null);
        }


        /// <summary>
        /// Adds Authentication setting <seealso cref="AuthenticationOptions.DefaultAuthenticateScheme"/> and <seealso cref="AuthenticationOptions.DefaultChallengeScheme"/> to <seealso cref="Scheme"/>
        /// </summary>
        /// <param name="services"><seealso cref="IServiceCollection"/></param>
        /// <param name="configureOptions"><seealso cref="AuthenticationOptions" /></param>
        /// <returns><seealso cref="AuthenticationBuilder"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [Obsolete("Removing in 8.x")]
        public virtual AuthenticationBuilder AddAuthentication(IServiceCollection services, Action<AuthenticationOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = Scheme;
                    opt.DefaultChallengeScheme = Scheme;
                    configureOptions?.Invoke(opt);
                });
        }

    }
}