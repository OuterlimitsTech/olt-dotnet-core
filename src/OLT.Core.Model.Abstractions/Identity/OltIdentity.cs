using OLT.Identity.Abstractions;

namespace OLT.Core
{
    /// <summary>
    /// Open Id Authorized User Interface to be injected using DI to provide current user info
    /// </summary>
    public abstract class OltIdentity : IOltIdentity
    {

        /// <summary>
        /// Is Anonymous Request (other properties will likely be null)
        /// </summary>
        public virtual bool IsAnonymous => Identity == null || Username == null;

        public abstract System.Security.Claims.ClaimsPrincipal? Identity { get; }

        /// <summary>
        /// End-User's Unique Name Id. Should be Used for the unique Id of the User
        /// </summary>
        /// <value><see cref="ClaimTypeNames.NameId"/></value>
        public virtual string? NameId => GetClaims(ClaimTypeNames.NameId).FirstOrDefault()?.Value ?? null;

        /// <summary>
        /// The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        /// <value><see cref="ClaimTypeNames.Subject"/></value>
        public virtual string? Subject => GetClaims(ClaimTypeNames.Subject).FirstOrDefault()?.Value;

        /// <summary>
        /// THIS PROPERTY IS REQUIRED!!!
        /// </summary>
        /// <remarks>
        /// Windows Identities -> <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>
        /// </remarks>
        /// <value><see cref="ClaimTypeNames.PreferredUsername"/> or <see cref="ClaimTypeNames.Username"/></value>
        public virtual string? Username =>
            GetClaims(ClaimTypeNames.PreferredUsername).FirstOrDefault()?.Value ??
            GetClaims(ClaimTypeNames.Username).FirstOrDefault()?.Value ??
            GetClaims(System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;  //Support for Legacy Microsoft Identities

        /// <summary>
        /// Given name(s) or first name(s) of the End-User. 
        /// </summary>
        /// <value><see cref="ClaimTypeNames.GivenName"/></value>
        public virtual string? FirstName => GetClaims(ClaimTypeNames.GivenName).FirstOrDefault()?.Value;

        /// <summary>
        /// Middle name(s) of the End-User.
        /// </summary>
        /// <value><see cref="ClaimTypeNames.MiddleName"/></value>
        public virtual string? MiddleName => GetClaims(ClaimTypeNames.MiddleName).FirstOrDefault()?.Value;


        /// <summary>
        /// Surname(s) or last name(s) of the End-User.
        /// </summary>
        /// <value><see cref="ClaimTypeNames.FamilyName"/></value>
        public virtual string? LastName => GetClaims(ClaimTypeNames.FamilyName).FirstOrDefault()?.Value;

        /// <summary>
        /// End-User's preferred e-mail address.
        /// </summary>
        /// <value><see cref="ClaimTypeNames.Email"/></value>
        public virtual string? Email => GetClaims(ClaimTypeNames.Email).FirstOrDefault()?.Value;

        /// <summary>
        /// End-User's preferred telephone number. E.164 [E.164] is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or +56 (2) 687 2400. 
        /// </summary>
        /// <value><see cref="ClaimTypeNames.PhoneNumber"/></value>
        public virtual string? Phone => GetClaims(ClaimTypeNames.PhoneNumber).FirstOrDefault()?.Value;

        /// <summary>
        /// True if the End-User's phone number has been verified; otherwise false. 
        /// </summary>
        /// <value><see cref="ClaimTypeNames.PhoneNumberVerified"/></value>
        public virtual bool? PhoneVerified
        {
            get
            {
                var value = GetClaims(ClaimTypeNames.PhoneNumberVerified).FirstOrDefault()?.Value;
                if (value == null) return null;
                if (string.IsNullOrWhiteSpace(value)) return null;


                if (bool.TryParse(value, out var parsed))
                {
                    return parsed;
                }

                return null;
            }
        }

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences.
        /// </summary>
        /// <value><see cref="ClaimTypeNames.Name"/></value>
        public virtual string? FullName => GetClaims(ClaimTypeNames.Name).FirstOrDefault()?.Value;


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
        /// Returns claim for <see cref="ClaimTypeNames.PreferredUsername"/> 
        /// </summary>
        public virtual string? GetDbUsername()
        {
            return this.Username ?? FullName;
        }

        /// <summary>
        /// Returns all claims <see cref="ClaimTypeNames.Role"/>
        /// </summary>
        public virtual List<System.Security.Claims.Claim> GetRoles()
        {
            return GetAllClaims().Where(p => p.Type == ClaimTypeNames.Role).ToList();
        }


        /// <summary>
        /// Checks if claim <see cref="ClaimTypeNames.Role"/> exists
        /// </summary>
        /// <param name="claimName"><see cref="ClaimTypeNames.Role"/></param>
        /// <returns></returns>
        public virtual bool HasRole(string? claimName)
        {
            return GetRoles().Exists(p => string.Equals(p.Value, claimName, StringComparison.OrdinalIgnoreCase));
        }


        public abstract bool HasRole<TRoleEnum>(params TRoleEnum[] roles) where TRoleEnum : Enum;

        ///// <summary>
        ///// Checks if claim <see cref="ClaimTypeNames.Role"/> exists this Enum <see cref="CodeAttribute"/> 
        ///// </summary>
        ///// <typeparam name="TRoleEnum"></typeparam>
        ///// <param name="roles"></param>
        ///// <returns></returns>
        //public virtual bool HasRole<TRoleEnum>(params TRoleEnum[] roles) where TRoleEnum : System.Enum
        //{
        //    return false;
        //    //return roles?.Any(role => HasRole(role.GetCodeEnum())) == true;
        //}


    }
}