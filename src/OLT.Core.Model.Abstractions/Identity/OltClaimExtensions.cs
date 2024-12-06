using OLT.Identity.Abstractions;

namespace OLT.Core
{
    public static class OltClaimExtensions
    {

        /// <summary>
        /// Build claims for the Properties
        /// <see cref="ClaimTypeNames.Name"/>, 
        /// <see cref="ClaimTypeNames.Email"/>, 
        /// <see cref="ClaimTypeNames.TokenType"/>, 
        /// <see cref="ClaimTypeNames.NameId"/>, 
        /// <see cref="ClaimTypeNames.PreferredUsername"/>, 
        /// <see cref="ClaimTypeNames.Username"/>, 
        /// <see cref="ClaimTypeNames.Role"/>
        /// </summary>
        /// <typeparam name="TNameModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.NameId"/>
        /// </remarks>   
        public static List<System.Security.Claims.Claim> ToClaims<TNameModel>(this OltAuthenticatedUserJson<TNameModel> model)
            where TNameModel : class, IOltPersonName, new()
        {
            ArgumentNullException.ThrowIfNull(model);

            var list = new List<System.Security.Claims.Claim>();            
            list.AddClaim(ClaimTypeNames.Name, model.FullName);
            list.AddClaim(ClaimTypeNames.Email, model.Email);
            list.AddClaim(ClaimTypeNames.TokenType, model.TokenType);
            list.AddClaim(ClaimTypeNames.NameId, model.NameId);
            list.AddClaim(ClaimTypeNames.PreferredUsername, model.Username);
            list.AddClaim(ClaimTypeNames.Username, model.Username);
            list.AddClaim(ClaimTypeNames.Nickname, model.Name.First);

            list.AddRange(model.Name.ToClaims());
            model.Roles.Where(value => !string.IsNullOrWhiteSpace(value)).ToList().ForEach(value => list.AddClaim(ClaimTypeNames.Role, value));
            model.Permissions.Where(value => !string.IsNullOrWhiteSpace(value)).ToList().ForEach(value => list.AddClaim(ClaimTypeNames.Role, value));
            return list;
        }

        /// <summary>
        /// Build claims <see cref="ClaimTypeNames.GivenName"/> <see cref="ClaimTypeNames.MiddleName"/> <see cref="ClaimTypeNames.FamilyName"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static List<System.Security.Claims.Claim> ToClaims(this IOltPersonName model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var list = new List<System.Security.Claims.Claim>();
                        
            list.AddClaim(ClaimTypeNames.GivenName, model.First);
            list.AddClaim(ClaimTypeNames.MiddleName, model.Middle);

            if (!string.IsNullOrWhiteSpace(model.Suffix))
            {
                list.AddClaim(ClaimTypeNames.FamilyName, $"{model.Last} {model.Suffix}");
            }
            else
            {
                list.AddClaim(ClaimTypeNames.FamilyName, model.Last);
            }
            
            return list;
        }

        /// <summary>
        /// Adds claim if <paramref name="value"/> has a value (not empty or null)
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AddClaim(this List<System.Security.Claims.Claim> claims, string type, string? value)
        {
            ArgumentNullException.ThrowIfNull(claims);
            ArgumentNullException.ThrowIfNullOrEmpty(type);

            if (value != null)
            {
                AddClaim(claims, new System.Security.Claims.Claim(type, value));
            }            
        }


        /// <summary>
        /// Adds claim if <see cref="System.Security.Claims.Claim.Value"/> has a value
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claim"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AddClaim(this List<System.Security.Claims.Claim> claims, System.Security.Claims.Claim claim)
        {
            ArgumentNullException.ThrowIfNull(claims);
            ArgumentNullException.ThrowIfNull(claim);

            if (!string.IsNullOrWhiteSpace(claim.Value))
            {
                claims.Add(claim);
            }
        }



    }
}
