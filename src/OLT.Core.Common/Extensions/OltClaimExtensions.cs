using OLT.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OLT.Core
{
    public static class OltClaimExtensions
    {

        /// <summary>
        /// Build claims <see cref="OltClaimTypes.Name"/>, <see cref="OltClaimTypes.Email"/>, <see cref="OltClaimTypes.AuthenticationMethod"/>, <see cref="OltClaimTypes.Upn"/>, <see cref="OltClaimTypes.NameIdentifier"/>, <see cref="OltClaimTypes.Role"/>
        /// </summary>
        /// <typeparam name="TNameModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static List<Claim> ToClaims<TNameModel>(this OltAuthenticatedUserJson<TNameModel> model)
            where TNameModel : class, IOltPersonName, new()
        {
            if (model == null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            var list = new List<Claim>();            
            list.AddClaim(OltClaimTypes.Name, model.FullName);
            list.AddClaim(OltClaimTypes.Email, model.Email);
            list.AddClaim(OltClaimTypes.TokenType, model.AuthenticationType);
            list.AddClaim(OltClaimTypes.UserPrincipalName, model.UserPrincipalName);
            list.AddClaim(OltClaimTypes.PreferredUsername, model.Username);

            list.AddRange(model.Name.ToClaims());
            list.AddRange(model.Roles.Select(value => new Claim(OltClaimTypes.Role, value)));
            list.AddRange(model.Permissions.Select(value => new Claim(OltClaimTypes.Role, value)));
            return list;
        }

        /// <summary>
        /// Build claims <see cref="OltClaimTypes.GivenName"/> <see cref="OltClaimTypes.MiddleName"/> <see cref="OltClaimTypes.Surname"/>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static List<Claim> ToClaims(this IOltPersonName model)
        {
            if (model == null)
            {
                throw new System.ArgumentNullException(nameof(model));
            }

            var list = new List<Claim>();
                        
            list.AddClaim(new Claim(OltClaimTypes.GivenName, model.First));
            list.AddClaim(new Claim(OltClaimTypes.MiddleName, model.Middle));

            if (!string.IsNullOrWhiteSpace(model.Suffix))
            {
                list.AddClaim(new Claim(OltClaimTypes.FamilyName, $"{model.Last} {model.Suffix}"));
            }
            else
            {
                list.AddClaim(new Claim(OltClaimTypes.FamilyName, model.Last));
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
        public static void AddClaim(this List<Claim> claims, string type, string value)
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
                AddClaim(claims, new Claim(type, value));
            }            
        }


        /// <summary>
        /// Adds claim if <see cref="Claim.Value"/> has a value
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="claim"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AddClaim(this List<Claim> claims, Claim claim)
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
