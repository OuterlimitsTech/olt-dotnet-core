namespace OLT.Constants
{

    /// <summary>
    /// List of registered claims from different sources
    /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
    /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
    /// https://github.com/openiddict/openiddict-core/blob/dev/src/OpenIddict.Abstractions/OpenIddictConstants.cs
    /// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/23808d5c7b11c3e0e9f202e48129c054e2b4f7ab/src/Microsoft.IdentityModel.JsonWebTokens/JwtRegisteredClaimNames.cs
    /// </summary>
    public static class OltClaimTypes
    {
        /// <summary>
        /// Identity Provider (typically used for local built-in providers)
        /// </summary>
        public const string IdentityProvider = "identityprovider";

        /// <summary>
        /// Access Token Validation
        /// </summary>
        /// <remarks>
        /// http://openid.net/specs/openid-connect-core-1_0.html#CodeIDToken
        /// </remarks>
        public const string AccessTokenHash = "at_hash";

        /// <summary>
        /// OPTIONAL. If Token is still Active
        /// </summary>
        /// <remarks>
        /// https://github.com/openiddict/openiddict-core/blob/dev/src/OpenIddict.Abstractions/OpenIddictConstants.cs
        /// </remarks>
        public const string Active = "active";

        /// <summary>
        /// OPTIONAL. Preferred postal address - The Address Claim represents a physical mailing address.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string Address = "address";

        /// <summary>
        /// OPTIONAL. The "aud" (audience) claim identifies the recipients that the JWT is intended for.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string Audience = "aud";

        /// <summary>
        /// OPTIONAL. Requested Authentication Context Class Reference values
        /// </summary>
        /// <remarks>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </remarks>
        public const string AuthenticationContextReference = "acr";

        /// <summary>
        /// OPTIONAL. Authentication Methods References.
        /// </summary>
        /// <remarks>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </remarks>
        public const string AuthenticationMethodReference = "amr";

        /// <summary>
        /// Time when the End-User authentication occurred.
        /// </summary>
        /// <remarks>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </remarks>
        public const string AuthenticationTime = "auth_time";

        /// <summary>
        /// End-User's Authorization Server
        /// </summary>
        public const string AuthorizationServer = "as";

        /// <summary>
        /// OPTIONAL. Authorized party - the party to which the ID Token was issued.
        /// </summary>
        /// <remarks>
        /// http://openid.net/specs/openid-connect-core-1_0.html#IDToken
        /// </remarks>
        public const string AuthorizedParty = "azp";

        /// <summary>
        /// OPTIONAL. End-User's birthday, represented as an ISO 8601:2004 [ISO8601‑2004] YYYY-MM-DD format.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Birthdate = "birthdate";

        /// <summary>
        /// Client Id of Application with Authorization Server
        /// </summary>
        public const string ClientId = "client_id";

        /// <summary>
        /// The c_hash in the ID Token enables Clients to prevent Authorization Code substitution.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#HybridIDToken
        /// </remarks>
        public const string CodeHash = "c_hash";


        /// <summary>
        /// Address Claim - Country name component.
        /// </summary>   
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string Country = "country";

        /// <summary>
        /// End-User's preferred e-mail address.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Email = "email";

        /// <summary>
        /// True if the End-User's e-mail address has been verified; otherwise false.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string EmailVerified = "email_verified";


        /// <summary>
        /// The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string ExpiresAt = "exp";


        /// <summary>
        /// Surname(s) or last name(s) of the End-User.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string FamilyName = "family_name";

        /// <summary>
        /// Address Claim - Full mailing address, formatted for display or use on a mailing label. This field MAY contain multiple lines, separated by newlines. Newlines can be represented either as a carriage return/line feed pair ("\r\n") or as a single line feed character ("\n")        
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string Formatted = "formatted";

        /// <summary>
        /// End-User's gender. Values defined by this specification are female and male. 
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Gender = "gender";

        /// <summary>
        /// Given name(s) or first name(s) of the End-User. 
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string GivenName = "given_name";

        /// <summary>
        /// The "iat" (issued at) claim identifies the time at which the JWT was issued.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string IssuedAt = "iat";

        /// <summary>
        /// The "iss" (issuer) claim identifies the principal that issued the JWT.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string Issuer = "iss";

        /// <summary>
        /// End-User's locale, represented as a BCP47 [RFC5646] language tag.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Locale = "locale";

        /// <summary>
        /// Address Claim - City or locality component.
        /// </summary>        
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string Locality = "locality";

        /// <summary>
        /// The "jti" (JWT ID) claim provides a unique identifier for the JWT.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string JwtId = "jti";

        /// <summary>
        /// End-User's Unique Key Id 
        /// </summary>
        public const string KeyId = "kid";

        /// <summary>
        /// Middle name(s) of the End-User.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string MiddleName = "middle_name";

        /// <summary>
        /// End-User's full name in displayable form including all name parts, possibly including titles and suffixes, ordered according to the End-User's locale and preferences.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Name = "name";

        /// <summary>
        /// End-User's Unique Name Id 
        /// </summary>
        public const string NameId = "nameid";

        [Obsolete("Change to NameId")]
        public const string NameIdentifier = "nameid";

        /// <summary>
        /// Casual name of the End-User that may or may not be the same as the given_name. For instance, a nickname value of Mike might be returned alongside a given_name value of Michael.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Nickname = "nickname";

        /// <summary>
        /// OPTIONAL. String value used to associate a Client session with an ID Token, and to mitigate replay attacks.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
        /// </remarks>
        public const string Nonce = "nonce";

        /// <summary>
        /// The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string NotBefore = "nbf";

        /// <summary>
        /// End-User's preferred telephone number. E.164 [E.164] is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or +56 (2) 687 2400. 
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string PhoneNumber = "phone_number";

        /// <summary>
        /// True if the End-User's phone number has been verified; otherwise false. 
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string PhoneNumberVerified = "phone_number_verified";

        /// <summary>
        /// URL of the End-User's profile picture. This URL MUST refer to an image file (for example, a PNG, JPEG, or GIF image file), rather than to a Web page containing an image.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Picture = "picture";

        /// <summary>
        /// Address Claim - Zip code or postal code component.
        /// </summary>   
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string PostalCode = "postal_code";

        /// <summary>
        /// Shorthand name by which the End-User wishes to be referred to at the RP, such as janedoe or j.doe.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string PreferredUsername = "preferred_username";


        /// <summary>
        /// URL of the End-User's profile page. The contents of this Web page SHOULD be about the End-User.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Profile = "profile";

        /// <summary>
        /// Address Claim - State, province, prefecture, or region component.
        /// </summary>   
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string Region = "region";

        ////public const string RequestForgeryProtection = "rfp";

        /// <summary>
        /// Roles
        /// </summary>
        public const string Role = "role";

        /// <summary>
        /// OpenID Connect Clients use scope values
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
        /// </remarks>
        public const string Scope = "scope";

        /// <summary>
        /// Full street address component, which MAY include house number, street name, Post Office Box, and multi-line extended street address information.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#AddressClaim
        /// </remarks>
        public const string StreetAddress = "street_address";

        /// <summary>
        /// The "sub" (subject) claim identifies the principal that is the subject of the JWT.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-4
        /// </remarks>
        public const string Subject = "sub";


        /// <summary>
        /// Token Type - Auth Type/Method (Bearer, API Key, etc.)
        /// </summary>
        public const string TokenType = "token_type";


        /// <summary>
        /// The "typ" (type) Header Parameter defined by [JWS] and [JWE] is used by JWT applications to declare the media type[IANA.MediaTypes] of this complete JWT.
        /// </summary>
        /// <remarks>
        /// https://datatracker.ietf.org/doc/html/rfc7519#section-5
        /// </remarks>
        public const string Typ = "typ";

        /// <summary>
        /// Time the End-User's information was last updated. Its value is a JSON number representing the number of seconds from 1970-01-01T0:0:0Z as measured in UTC until the date/time.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string UpdatedAt = "updated_at";

        /// <summary>
        /// The username used to login, usually the same as to the <see cref="PreferredUsername"/> 
        /// </summary>
        public const string Username = "username";

        /// <summary>
        /// URL of the End-User's Web page or blog. This Web page SHOULD contain information published by the End-User or an organization that the End-User is affiliated with.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Website = "website";

        /// <summary>
        /// String from zoneinfo [zoneinfo] time zone database representing the End-User's time zone. For example, Europe/Paris or America/Los_Angeles.
        /// </summary>
        /// <remarks>
        /// https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
        /// </remarks>
        public const string Zoneinfo = "zoneinfo";

    }
}