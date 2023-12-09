using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    /// <summary>
    /// Open Id Authorized User Interface to be injected using DI to provide current user info
    /// </summary>
    public abstract class OltIdentity : OltDisposable, IOltIdentity
    {

        /// <summary>
        /// Is Anonymous Request (other properties will likely be null)
        /// </summary>
        public virtual bool IsAnonymous => Identity == null || Username == null;

        public abstract System.Security.Claims.ClaimsPrincipal Identity { get; }

        /// <summary>
        /// End-User's Unique Name Id. Should be Used for the unique Id of the User
        /// </summary>
        /// <value><see cref="OltClaimTypes.NameId"/></value>
        public virtual string? NameId => GetClaims(OltClaimTypes.NameId).FirstOrDefault()?.Value ?? null;

        /// <summary>
        /// The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Subject"/></value>
        public virtual string? Subject => GetClaims(OltClaimTypes.Subject).FirstOrDefault()?.Value;

        /// <summary>
        /// THIS PROPERTY IS REQUIRED!!!
        /// </summary>
        /// <remarks>
        /// Windows Identities -> <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </remarks>
        /// <value><see cref="OltClaimTypes.PreferredUsername"/> or <see cref="OltClaimTypes.Username"/></value>
        public virtual string? Username => 
            GetClaims(OltClaimTypes.PreferredUsername).FirstOrDefault()?.Value ??
            GetClaims(OltClaimTypes.Username).FirstOrDefault()?.Value ??
            GetClaims(System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;  //Support for Legacy Microsoft Identities

        /// <summary>
        /// Given name(s) or first name(s) of the End-User. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.GivenName"/></value>
        public virtual string? FirstName => GetClaims(OltClaimTypes.GivenName).FirstOrDefault()?.Value;

        /// <summary>
        /// Middle name(s) of the End-User.
        /// </summary>
        /// <value><see cref="OltClaimTypes.MiddleName"/></value>
        public virtual string? MiddleName => GetClaims(OltClaimTypes.MiddleName).FirstOrDefault()?.Value;


        /// <summary>
        /// Surname(s) or last name(s) of the End-User.
        /// </summary>
        /// <value><see cref="OltClaimTypes.FamilyName"/></value>
        public virtual string? LastName => GetClaims(OltClaimTypes.FamilyName).FirstOrDefault()?.Value;

        /// <summary>
        /// End-User's preferred e-mail address.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Email"/></value>
        public virtual string? Email => GetClaims(OltClaimTypes.Email).FirstOrDefault()?.Value;

        /// <summary>
        /// End-User's preferred telephone number. E.164 [E.164] is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or +56 (2) 687 2400. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.PhoneNumber"/></value>
        public virtual string? Phone => GetClaims(OltClaimTypes.PhoneNumber).FirstOrDefault()?.Value;

        /// <summary>
        /// True if the End-User's phone number has been verified; otherwise false. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.PhoneNumberVerified"/></value>
        public virtual bool? PhoneVerified => GetClaims(OltClaimTypes.PhoneNumberVerified).FirstOrDefault()?.Value?.ToBool();

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Name"/></value>
        public virtual string? FullName => GetClaims(OltClaimTypes.Name).FirstOrDefault()?.Value;

        /// <summary>
        /// The identifier of the user - Default Claim 
        /// -> Windows Identities <see cref="System.Security.Claims.ClaimTypes.Upn"/>
        /// -> Falls Back to <see cref="NameId"/>
        /// </summary>
        [Obsolete("Move To NameId - This is a legacy AD claim")]
        public virtual string? UserPrincipalName => 
            GetClaims(System.Security.Claims.ClaimTypes.Upn).FirstOrDefault()?.Value ?? //Support for Legacy Microsoft Identities
            NameId; 

        /// <summary>
        /// Get all claims for given <see cref="Identity"/>
        /// </summary>
        public virtual List<System.Security.Claims.Claim> GetAllClaims()
        {
            if (Identity != null)
            {
                return Identity.Claims.ToList();
            }
            return new List<System.Security.Claims.Claim>();
        }

        /// <summary>
        /// Get all claims for giving value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual List<System.Security.Claims.Claim> GetClaims(string type)
        {
            return GetAllClaims().Where(p => p.Type == type).ToList();
        }


        /// <summary>
        /// Returns claim for <see cref="OltClaimTypes.PreferredUsername"/> 
        /// </summary>
        public virtual string? GetDbUsername()
        {
            return this.Username ?? FullName;
        }

        /// <summary>
        /// Returns all claims <see cref="OltClaimTypes.Role"/>
        /// </summary>
        public virtual List<System.Security.Claims.Claim> GetRoles()
        {
            return GetAllClaims().Where(p => p.Type == OltClaimTypes.Role).ToList();
        }


        /// <summary>
        /// Checks if claim <see cref="OltClaimTypes.Role"/> exists
        /// </summary>
        /// <param name="claimName"><see cref="OltClaimTypes.Role"/></param>
        /// <returns></returns>
        public virtual bool HasRole(string claimName)
        {
            return GetRoles().Exists(p => string.Equals(p.Value, claimName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if claim <see cref="OltClaimTypes.Role"/> exists this Enum <see cref="CodeAttribute"/> 
        /// </summary>
        /// <typeparam name="TRoleEnum"></typeparam>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool HasRole<TRoleEnum>(params TRoleEnum[] roles) where TRoleEnum : System.Enum
        {
            return roles?.Any(role => HasRole(role.GetCodeEnum())) == true;
        }


    }
}