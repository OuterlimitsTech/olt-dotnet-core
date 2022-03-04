using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

        public abstract ClaimsPrincipal Identity { get; }

        /// <summary>
        /// Claim <see cref="ClaimTypes.NameIdentifier"/>
        /// </summary>
        public virtual string Username => GetClaims(ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.GivenName"/>
        /// </summary>
        public virtual string FirstName => GetClaims(ClaimTypes.GivenName).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="OltClaimTypes.MiddleName"/>
        /// </summary>
        public virtual string MiddleName => GetClaims(OltClaimTypes.MiddleName).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.Surname"/>
        /// </summary>
        public virtual string LastName => GetClaims(ClaimTypes.Surname).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.Email"/>
        /// </summary>
        public virtual string Email => GetClaims(ClaimTypes.Email).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.HomePhone"/>
        /// </summary>
        public virtual string Phone => GetClaims(ClaimTypes.HomePhone).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.Name"/>
        /// </summary>
        public virtual string FullName => GetClaims(ClaimTypes.Name).FirstOrDefault()?.Value;

        /// <summary>
        /// Claim <see cref="ClaimTypes.Upn"/>
        /// </summary>
        public virtual string UserPrincipalName => GetClaims(ClaimTypes.Upn).FirstOrDefault()?.Value;

        /// <summary>
        /// Get all claims for given <see cref="Identity"/>
        /// </summary>
        public virtual List<Claim> GetAllClaims()
        {
            if (Identity != null)
            {
                return Identity.Claims.ToList();
            }
            return new List<Claim>();
        }

        /// <summary>
        /// Get all claims for giving value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual List<Claim> GetClaims(string type)
        {
            return GetAllClaims().Where(p => p.Type == type).ToList();
        }


        /// <summary>
        /// Returns claim for <see cref="ClaimTypes.NameIdentifier"/> 
        /// </summary>
        public virtual string GetDbUsername()
        {
            return this.Username ?? FullName;
        }

        /// <summary>
        /// Returns all claims <see cref="ClaimTypes.Role"/>
        /// </summary>
        public virtual List<Claim> GetRoles()
        {
            return GetAllClaims().Where(p => p.Type == ClaimTypes.Role).ToList();
        }


        /// <summary>
        /// Checks if claim <see cref="ClaimTypes.Role"/> exists
        /// </summary>
        /// <param name="claimName"><see cref="ClaimTypes.Role"/></param>
        /// <returns></returns>
        public virtual bool HasRole(string claimName)
        {
            return GetRoles().Any(p => string.Equals(p.Value, claimName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if claim <see cref="ClaimTypes.Role"/> exists this Enum <see cref="CodeAttribute"/> 
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