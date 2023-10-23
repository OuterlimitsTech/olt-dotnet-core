using System;
using Microsoft.AspNetCore.Authentication;

namespace OLT.AspNetCore.Authentication
{
    [Obsolete("Removing in 8.x")]
    public abstract class OltAuthenticationSchemeBuilder<TSchemeOption> : OltAuthenticationBuilder, IOltAuthenticationSchemeBuilder<TSchemeOption>
        where TSchemeOption : AuthenticationSchemeOptions
    {

        /// <summary>
        /// Adds Authentication 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [Obsolete("Removing in 8.x")]
        public virtual AuthenticationBuilder AddScheme(AuthenticationBuilder builder)
        {
            return AddScheme(builder, null);
        }

        /// <summary>
        /// Adds Authentication 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        [Obsolete("Removing in 8.x")]
        public abstract AuthenticationBuilder AddScheme(AuthenticationBuilder builder, Action<TSchemeOption> configureOptions);

    }
}