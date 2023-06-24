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
        /// Default Claim <see cref="OltClaimTypes.PreferredUsername"/> (Windows Identities <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>)
        /// </summary>
        public virtual string Username => 
            GetClaims(OltClaimTypes.PreferredUsername).FirstOrDefault()?.Value ??
            GetClaims(System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;  //Support for Legacy Microsoft Identities

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.GivenName"/>
        /// </summary>
        public virtual string FirstName => GetClaims(OltClaimTypes.GivenName).FirstOrDefault()?.Value;

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.MiddleName"/>
        /// </summary>
        public virtual string MiddleName => GetClaims(OltClaimTypes.MiddleName).FirstOrDefault()?.Value;

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.FamilyName"/> 
        /// </summary>
        public virtual string LastName => GetClaims(OltClaimTypes.FamilyName).FirstOrDefault()?.Value;

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.Email"/>
        /// </summary>
        public virtual string Email => GetClaims(OltClaimTypes.Email).FirstOrDefault()?.Value;

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.PhoneNumber"/>
        /// </summary>
        public virtual string Phone => GetClaims(OltClaimTypes.PhoneNumber).FirstOrDefault()?.Value;

        /// <summary>
        /// Default Claim <see cref="OltClaimTypes.Name"/>
        /// </summary>
        public virtual string FullName => GetClaims(OltClaimTypes.Name).FirstOrDefault()?.Value;

        /// <summary>
        /// The identifier of the user - Default Claim <see cref="OltClaimTypes.Subject"/> (legacy <see cref="OltClaimTypes.UserPrincipalName"/>) (Windows Identities <see cref="System.Security.Claims.ClaimTypes.Upn"/>)
        /// </summary>
        public virtual string UserPrincipalName => 
            GetClaims(OltClaimTypes.UserPrincipalName).FirstOrDefault()?.Value ?? 
            GetClaims(OltClaimTypes.Subject).FirstOrDefault()?.Value ??
            GetClaims(System.Security.Claims.ClaimTypes.Upn).FirstOrDefault()?.Value; //Support for Legacy Microsoft Identities

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
        public virtual string GetDbUsername()
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
            return GetRoles().Any(p => string.Equals(p.Value, claimName, StringComparison.OrdinalIgnoreCase));
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