using System;
using Microsoft.AspNetCore.Authentication;

namespace OLT.AspNetCore.Authentication
{
    [Obsolete("Removing in 8.x")]
    public interface IOltAuthenticationSchemeBuilder
    {
        AuthenticationBuilder AddScheme(AuthenticationBuilder builder);
    }

    [Obsolete("Removing in 8.x")]
    public interface IOltAuthenticationSchemeBuilder<out TSchemeOption> : IOltAuthenticationSchemeBuilder, IOltAuthenticationBuilder
        where TSchemeOption : AuthenticationSchemeOptions
    {        
        AuthenticationBuilder AddScheme(AuthenticationBuilder builder, Action<TSchemeOption> configureOptions);
    }
}