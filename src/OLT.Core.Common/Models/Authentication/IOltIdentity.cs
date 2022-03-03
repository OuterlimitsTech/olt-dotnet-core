using System.Collections.Generic;
using System.Security.Claims;

namespace OLT.Core
{
    /// <summary>
    /// Open Id Authorized User Interface to be injected using DI to provide current user info
    /// </summary>
    public interface IOltIdentity : IOltDbAuditUser, IOltIdentityUser
    {
        /// <summary>
        /// Is Anonymous Request (other properties will likely be null)
        /// </summary>
        bool IsAnonymous { get; }

        /// <summary>
        /// Get all claims for Identity
        /// </summary>
        List<Claim> GetAllClaims();

        /// <summary>
        /// Get all claims for giving value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<Claim> GetClaims(string type);

        /// <summary>
        /// Returns all claims <see cref="ClaimTypes.Role"/>
        /// </summary>
        List<Claim> GetRoles();

        /// <summary>
        /// Checks if claim <see cref="ClaimTypes.Role"/> exists
        /// </summary>
        /// <param name="claimName"><see cref="ClaimTypes.Role"/></param>
        /// <returns></returns>
        bool HasRole(string claimName);

        /// <summary>
        /// Checks if claim <see cref="ClaimTypes.Role"/> exists this Enum <see cref="CodeAttribute"/> 
        /// </summary>
        /// <typeparam name="TRoleEnum"></typeparam>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool HasRole<TRoleEnum>(params TRoleEnum[] roles) where TRoleEnum : System.Enum;

    }
}