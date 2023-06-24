using OLT.Constants;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public static class OltClaimExtensions
    {

        /// <summary>
        /// Build claims <see cref="OltClaimTypes.Name"/>, <see cref="OltClaimTypes.Email"/>, <see cref="OltClaimTypes.TokenType"/>, <see cref="OltClaimTypes.UserPrincipalName"/>, <see cref="OltClaimTypes.PreferredUsername"/>, <see cref="OltClaimTypes.Role"/>
        /// </summary>
        /// <typeparam name="TNameModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static List<System.Security.Claims.Claim> ToClaims<TNameModel>(this OltAuthenticatedUserJson<TNameModel> model)
            where TNameModel : class, IOltPersonName, new()
        {
            if (model == null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            var list = new List<System.Security.Claims.Claim>();            
            list.AddClaim(OltClaimTypes.Name, model.FullName);
            list.AddClaim(OltClaimTypes.Email, model.Email);
            list.AddClaim(OltClaimTypes.TokenType, model.AuthenticationType);
            list.AddClaim(OltClaimTypes.UserPrincipalName, model.UserPrincipalName);
            list.AddClaim(OltClaimTypes.PreferredUsername, model.Username);

            list.AddRange(model.Name.ToClaims());
            list.AddRange(model.Roles.Select(value => new System.Security.Claims.Claim(OltClaimTypes.Role, value)));
            list.AddRange(model.Permissions.Select(value => new System.Security.Claims.Claim(OltClaimTypes.Role, value)));
            return list;
        }

        /// <summary>
        /// Build claims <see cref="OltClaimTypes.GivenName"/> <see cref="OltClaimTypes.MiddleName"/> <see cref="OltClaimTypes.FamilyName"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static List<System.Security.Claims.Claim> ToClaims(this IOltPersonName model)
        {
            if (model == null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            var list = new List<System.Security.Claims.Claim>();
                        
            list.AddClaim(new System.Security.Claims.Claim(OltClaimTypes.GivenName, model.First));
            list.AddClaim(new System.Security.Claims.Claim(OltClaimTypes.MiddleName, model.Middle));

            if (!string.IsNullOrWhiteSpace(model.Suffix))
            {
                list.AddClaim(new System.Security.Claims.Claim(OltClaimTypes.FamilyName, $"{model.Last} {model.Suffix}"));
            }
            else
            {
                list.AddClaim(new System.Security.Claims.Claim(OltClaimTypes.FamilyName, model.Last));
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
        public static void AddClaim(this List<System.Security.Claims.Claim> claims, string type, string value)
        {
            if (claims == null)
            {
                throw new System.ArgumentNullException(nameof(claims));
            }

            if (type == null)
            {
                throw new System.ArgumentNullException(nameof(type));
            }
            if (value != null)
            {
                AddClaim(claims, new System.Security.Claims.Claim(type, value));
            }            
        }


        /// <summary>
        /// Adds claim if <see cref="Claim.Value"/> has a value
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claim"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AddClaim(this List<System.Security.Claims.Claim> claims, System.Security.Claims.Claim claim)
        {
            if (claims == null)
            {
                throw new System.ArgumentNullException(nameof(claims));
            }

            if (claim == null)
            {
                throw new System.ArgumentNullException(nameof(claim));
            }

            if (!string.IsNullOrWhiteSpace(claim.Value))
            {
                claims.Add(claim);
            }
        }



    }
}
