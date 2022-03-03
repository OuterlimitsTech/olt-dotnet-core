using System;
using System.Collections.Generic;

namespace OLT.Core
{
    /// <summary>
    /// Standardized User Tokan Model
    /// </summary>
    /// <typeparam name="TNameModel"></typeparam>
    public abstract class OltAuthenticatedUserTokenJson<TNameModel> 
        where TNameModel : class, IOltPersonName, new()
    {

        /// <summary>
        /// User Principal Name (usually the Id of the user) 
        /// </summary>
        /// <remarks>
        /// UPN
        /// </remarks>
        public virtual int UserPrincipalName { get; set; }
        public virtual string Username { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string FullName => Name.FullName;
        public virtual TNameModel Name { get; set; } = new TNameModel();

        public virtual string AuthenticationType { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTimeOffset Issued { get; set; }
        public virtual DateTimeOffset Expires { get; set; }
        public virtual string ExpiresIn => $"{(Expires - Issued).TotalSeconds}";

        public virtual IEnumerable<string> Roles { get; set; }
        public virtual IEnumerable<string> Permissions { get; set; }

    }
}