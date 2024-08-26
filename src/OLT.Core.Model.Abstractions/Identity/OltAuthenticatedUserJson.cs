using OLT.Constants;
using System;
using System.Collections.Generic;

namespace OLT.Core
{
    /// <summary>
    /// Standardized Authenticated User Model used by the Angular
    /// <see href="https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/ee913589(v=ws.11)?redirectedfrom=MSDN#what-are-claim-types"/>
    /// </summary>
    /// <typeparam name="TNameModel"></typeparam>
    /// <remarks>
    /// Use extension to convert to claims <see cref="OltClaimExtensions.ToClaims"/>
    /// </remarks>
    public class OltAuthenticatedUserJson<TNameModel>
        where TNameModel : class, IOltPersonName, new()
    {

        /// <summary>
        /// User Principal Name (usually the Id of the user) 
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.NameId"/>
        /// </remarks>
        [Obsolete("Move to NameId")]
        public virtual string? UserPrincipalName
        {
            get
            {
                return NameId;
            }
            set
            {
                NameId = value;
            }
        }

        /// <summary>
        /// User's Unique Identifer for the Provider
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.NameId"/>
        /// </remarks>        
        public virtual string? NameId { get; set; }

        /// <summary>
        /// Username of user
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.PreferredUsername"/>
        /// </remarks>
        public virtual string? Username { get; set; }

        /// <summary>
        /// Email of user
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.Email"/>
        /// </remarks>
        public virtual string? Email { get; set; }


        /// <summary>
        /// Full name of user using <see cref="IOltPersonName.FullName"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.Name"/>
        /// </remarks>
        public virtual string? FullName => Name.FullName;


        /// <summary>
        /// Full name of user using <see cref="IOltPersonName.FullName"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.GivenName"/> <see cref="OltClaimTypes.MiddleName"/> <see cref="OltClaimTypes.FamilyName"/>
        /// </remarks>
        public virtual TNameModel Name { get; set; } = new TNameModel();


        /// <summary>
        /// Auth Type/Method (Bearer, API Key, etc.)
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.TokenType"/>
        /// </remarks>     
        [Obsolete("Move to TokenType")]
        public virtual string? AuthenticationType
        {
            get
            {
                return TokenType;
            }
            set
            {
                TokenType = value;
            }
        }

        /// <summary>
        /// Auth Type/Method (Bearer, API Key, etc.)
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.TokenType"/>
        /// </remarks>        
        public virtual string? TokenType { get; set; }

        /// <summary>
        /// Roles for User
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.Role"/>
        /// </remarks>   
        public virtual List<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// Permissions for User
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.Role"/>
        /// </remarks>   
        public virtual List<string> Permissions { get; set; } = new List<string>();
    }
}