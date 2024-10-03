using OLT.Constants;
using System;

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
        /// <value><see cref="OltClaimTypes.NameId"/></value>
        [Obsolete("Move to Name Id")]
        string? UserPrincipalName { get; }

        /// <summary>
        /// End-User's Unique Name Id. Should be Used for the unique Id of the User
        /// </summary>
        /// <value><see cref="OltClaimTypes.NameId"/></value>
        string? NameId { get; }

        /// <summary>
        /// The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Subject"/></value>
        string? Subject { get; }

        /// <summary>
        /// THIS PROPERTY IS REQUIRED!!!
        /// </summary>
        /// <remarks>
        /// Windows Identities -> <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </remarks>
        /// <value><see cref="OltClaimTypes.PreferredUsername"/> or <see cref="OltClaimTypes.Username"/></value>
        string? Username { get; }

        /// <summary>
        /// Given name(s) or first name(s) of the End-User. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.GivenName"/></value>
        string? FirstName { get; }

        /// <summary>
        /// Middle name(s) of the End-User.
        /// </summary>
        /// <value><see cref="OltClaimTypes.MiddleName"/></value>
        string? MiddleName { get; }

        /// <summary>
        /// Surname(s) or last name(s) of the End-User.
        /// </summary>
        /// <value><see cref="OltClaimTypes.FamilyName"/></value>
        string? LastName { get; }


        /// <summary>
        /// End-User's preferred e-mail address.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Email"/></value>
        string? Email { get; }

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences.
        /// </summary>
        /// <value><see cref="OltClaimTypes.Name"/></value>
        string? FullName { get; }

        /// <summary>
        /// End-User's preferred telephone number. E.164 [E.164] is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or +56 (2) 687 2400. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.PhoneNumber"/></value>
        string? Phone { get; }

        /// <summary>
        /// True if the End-User's phone number has been verified; otherwise false. 
        /// </summary>
        /// <value><see cref="OltClaimTypes.PhoneNumberVerified"/></value>
        bool? PhoneVerified { get; }
    }
}