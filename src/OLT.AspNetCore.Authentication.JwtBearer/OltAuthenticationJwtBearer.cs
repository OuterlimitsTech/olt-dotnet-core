using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OLT.Constants;
using OLT.Core;
using System;
using System.Text;

namespace OLT.AspNetCore.Authentication
{
    public class OltAuthenticationJwtBearer : IOltAuthenticationJwtBearer
    {
        public OltAuthenticationJwtBearer(string jwtSecret)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(jwtSecret);
            JwtSecret = jwtSecret;
        }

        /// <summary>
        /// Secret used to encrypt/decrypt jwt signature <seealso cref="TokenValidationParameters.IssuerSigningKey"/>
        /// </summary>
        public virtual string JwtSecret { get; }

        /// <summary>
        /// Gets or sets if HTTPS is required for the metadata address or authority.
        /// </summary>
        /// <remarks>
        /// The default is true. This should be disabled only in development environments.
        /// </remarks>
        public virtual bool RequireHttpsMetadata { get; set; } = true;


        /// <summary>
        /// Gets or sets a boolean to control if the audience will be validated during token validation.
        /// </summary>
        /// <remarks>
        ///  Validation of the audience, mitigates forwarding attacks. For example, a site
        ///  that receives a token, could not replay it to another side. A forwarded token
        ///  would contain the audience of the original site. This boolean only applies to
        ///  default audience validation. If Microsoft.IdentityModel.Tokens.TokenValidationParameters.AudienceValidator
        ///  is set, it will be called regardless of whether this property is true or false.
        /// </remarks>
        public virtual bool ValidateIssuer { get; set; }

        /// <summary>
        /// Gets or sets a boolean to control if the original token should be saved after the security token is validated.
        /// </summary>
        /// <remarks>
        /// The runtime will consult this value and save the original token that was validated.
        /// </remarks>
        public virtual bool ValidateAudience { get; set; }

        /// <summary>
        /// Builds Jwt Bearer Token Authentication Scheme
        /// </summary>
        /// <param name="builder"><seealso cref="AuthenticationBuilder"/></param>
        /// <param name="configureOptions"><seealso cref="JwtBearerOptions"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual AuthenticationBuilder AddScheme(AuthenticationBuilder builder, Action<JwtBearerOptions>? configureOptions)
        {
            ArgumentNullException.ThrowIfNull(builder);
            
            builder.AddJwtBearer(opt =>
            {

#pragma warning disable S125
                //opt.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        //var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                //        //var userId = int.Parse(context.Principal.Identity.Name);
                //        //if (context.Principal.Identity is ClaimsIdentity identity)
                //        //{
                //        //    var userId = Convert.ToInt32(identity.Claims.FirstOrDefault(p => p.Type == OltClaimTypes.Upn)?.Value);
                //        //}

                //        //var user = userService.GetById(userId);
                //        //if (user == null)
                //        //{
                //        //    // return unauthorized if user no longer exists
                //        //    context.Fail("Unauthorized");
                //        //}
                //        return Task.CompletedTask;
                //    }
                //};
#pragma warning restore S125
                opt.MapInboundClaims = false;
                opt.RequireHttpsMetadata = RequireHttpsMetadata;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OltClaimTypes.Name,
                    RoleClaimType = OltClaimTypes.Role,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecret)),
                    ValidateIssuer = ValidateIssuer,
                    ValidateAudience = ValidateAudience
                };

                configureOptions?.Invoke(opt);
            });

            return builder;
        }


    }
}