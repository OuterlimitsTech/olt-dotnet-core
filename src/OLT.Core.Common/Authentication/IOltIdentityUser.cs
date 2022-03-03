using System.Security.Claims;

namespace OLT.Core
{
    public interface IOltIdentityUser
    {
        /// <summary>
        /// User principal name (UPN)
        /// </summary>
        /// <remarks>
        /// To Be used for local system Id
        /// </remarks>
        /// <value><see cref="ClaimTypes.Upn"/></value>
        string UserPrincipalName { get; }

        /// <summary>
        /// THIS PROPERTY IS REQUIRED!!! Username of User 
        /// </summary>
        /// <remarks>
        /// Unique Id from the OAuth Provider or Username provided from Token of User
        /// </remarks>
        /// <value><see cref="ClaimTypes.NameIdentifier"/></value>
        string Username { get; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <value><see cref="ClaimTypes.GivenName"/></value>
        string FirstName { get; }

        /// <summary>
        /// Email Address of User
        /// </summary>
        /// <value><see cref="OltClaimTypes.MiddleName"/></value>
        string MiddleName { get; }

        /// <summary>
        /// Email Address of User
        /// </summary>
        /// <value><see cref="ClaimTypes.Surname"/></value>
        string LastName { get; }


        /// <summary>
        /// Email Address of User
        /// </summary>
        /// <value><see cref="ClaimTypes.Email"/></value>
        string Email { get; }


        /// <summary>
        /// Full Name of User
        /// </summary>
        /// <value><see cref="ClaimTypes.Name"/></value>
        string FullName { get; }
    }
}