using System;

namespace OLT.Core
{
    /// <summary>
    /// Standardized User JWT Tokan Model used by the Angular
    /// </summary>
    /// <typeparam name="TNameModel"></typeparam>
    public class OltAuthenticatedUserJwtTokenJson<TNameModel> : OltAuthenticatedUserJson<TNameModel>
        where TNameModel : class, IOltPersonName, new()
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Token Issued 
        /// </summary>
        public virtual DateTimeOffset? Issued { get; set; }

        /// <summary>
        /// Token Expires 
        /// </summary>
        public virtual DateTimeOffset? Expires { get; set; }

        /// <summary>
        /// Difference between <see cref="Expires"/> and <see cref="Issued"/> in Seconds
        /// </summary>
        public virtual double? ExpiresIn => Expires.HasValue && Issued.HasValue ? (Expires.Value - Issued.Value).TotalSeconds : new double?();
    }
}