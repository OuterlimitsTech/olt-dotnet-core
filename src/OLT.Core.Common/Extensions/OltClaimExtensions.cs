using OLT.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OLT.Core
{
    public static class OltClaimExtensions
    {

        /// <summary>
        /// Build claims <see cref="ClaimTypes.Name"/>, <see cref="ClaimTypes.Email"/>, <see cref="ClaimTypes.AuthenticationMethod"/>, <see cref="ClaimTypes.Upn"/>, <see cref="ClaimTypes.NameIdentifier"/>, <see cref="ClaimTypes.Role"/>
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
            list.AddClaim(ClaimTypes.Name, model.FullName);
            list.AddClaim(ClaimTypes.Email, model.Email);
            list.AddClaim(ClaimTypes.AuthenticationMethod, model.AuthenticationType);
            list.AddClaim(ClaimTypes.Upn, model.UserPrincipalName);
            list.AddClaim(ClaimTypes.NameIdentifier, model.Username);

            list.AddRange(model.Name.ToClaims());
            list.AddRange(model.Roles.Select(value => new Claim(ClaimTypes.Role, value)));
            list.AddRange(model.Permissions.Select(value => new Claim(ClaimTypes.Role, value)));
            return list;
        }

        /// <summary>
        /// Build claims <see cref="ClaimTypes.GivenName"/> <see cref="OltClaimTypes.MiddleName"/> <see cref="ClaimTypes.Surname"/>
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
                        
            list.AddClaim(new Claim(ClaimTypes.GivenName, model.First));
            list.AddClaim(new Claim(OltClaimTypes.MiddleName, model.Middle));

            if (!string.IsNullOrWhiteSpace(model.Suffix))
            {
                list.AddClaim(new Claim(ClaimTypes.Surname, $"{model.Last} {model.Suffix}"));
            }
            else
            {
                list.AddClaim(new Claim(ClaimTypes.Surname, model.Last));
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
