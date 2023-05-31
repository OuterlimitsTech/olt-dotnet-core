using static System.Net.WebRequestMethods;

namespace OLT.Constants
{

    /// <summary>
    /// List of registered claims from different sources
    /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
    /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
    /// https://github.com/openiddict/openiddict-core/blob/dev/src/OpenIddict.Abstractions/OpenIddictConstants.cs
    /// </summary>
    public static class OltClaimTypes
    {
        /// <summary>
        /// Identity Provider (typically used for local built-in providers)
        /// </summary>
        public const string IdentityProvider = "identityprovider";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
        /// </summary>
        public const string AccessTokenHash = "at_hash";

        /// <summary>
        /// If Token is still Active
        /// </summary>
        public const string Active = "active";

        /// <summary>
        /// Preferred postal address - https://openid.net/specs/openid-connect-core-1_0.html
        /// </summary>
        public const string Address = "address";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Audience = "aud";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthenticationContextReference = "acr";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthenticationMethodReference = "amr";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthenticationTime = "auth_time";

        /// <summary>
        /// End-User's Authorization Server
        /// </summary>
        public const string AuthorizationServer = "as";

        /// <summary>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </summary>
        public const string AuthorizedParty = "azp";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Birthdate = "birthdate";

        /// <summary>
        /// Client Id of Application with Authorization Server
        /// </summary>
        public const string ClientId = "client_id";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken
        /// </summary>
        public const string CodeHash = "c_hash";


        /// <summary>
        /// Address Claim - Country name component.
        /// </summary>   
        public const string Country = "country";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// If End-User's Email Address is verified
        /// </summary>
        public const string EmailVerified = "email_verified";


        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string ExpiresAt = "exp";


        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string FamilyName = "family_name";

        /// <summary>
        /// Address Claim - Full mailing address, formatted for display or use on a mailing label. This field MAY contain multiple lines, separated by newlines. Newlines can be represented either as a carriage return/line feed pair ("\r\n") or as a single line feed character ("\n")        
        /// </summary>
        public const string Formatted = "formatted";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Gender = "gender";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string GivenName = "given_name";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string IssuedAt = "iat";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Issuer = "iss";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Locale = "locale";

        /// <summary>
        /// Address Claim - City or locality component.
        /// </summary>        
        public const string Locality = "locality";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string JwtId = "jti";

        /// <summary>
        /// End-User's Unique Key Id 
        /// </summary>
        public const string KeyId = "kid";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string MiddleName = "middle_name";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// End-User's Unique Name Id 
        /// </summary>
        public const string NameId = "nameid";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Nickname = "nickname";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </summary>
        public const string Nonce = "nonce";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string NotBefore = "nbf";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string PhoneNumber = "phone_number";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string PhoneNumberVerified = "phone_number_verified";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Picture = "picture";

        /// <summary>
        /// Address Claim - Zip code or postal code component.
        /// </summary>   
        public const string PostalCode = "postal_code";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string PreferredUsername = "preferred_username";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Profile = "profile";

        /// <summary>
        /// Address Claim - State, province, prefecture, or region component.
        /// </summary>   
        public const string Region = "region";

        public const string RequestForgeryProtection = "rfp";
        public const string Role = "role";
        public const string Scope = "scope";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaims
        /// </summary>
        public const string StreetAddress = "street_address";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </summary>
        public const string Subject = "sub";


        //public const string TargetLinkUri = "target_link_uri";

        ///// <summary>
        ///// Token Type
        ///// </summary>
        //public const string TokenType = "token_type";

        ///// <summary>
        ///// Token Usage Type
        ///// </summary>
        //public const string TokenUsage = "token_usage";

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-5
        /// </summary>
        public const string Typ = "typ";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string UpdatedAt = "updated_at";


        //public const string Username = "username";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Website = "website";

        /// <summary>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </summary>
        public const string Zoneinfo = "zoneinfo";

    }
}