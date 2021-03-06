using OLT.Constants;
using System.Security.Claims;

namespace OLT.Core
{
    public class OltPersonName : IOltPersonName
    {
        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypes.GivenName"/>
        /// </remarks>
        public virtual string First { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.MiddleName"/>
        /// </remarks>
        public virtual string Middle { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypes.Surname"/>
        /// </remarks>
        public virtual string Last { get; set; }

        /// <summary>
        /// Name Suffix (Jr, Sr, II, III, IV, V)
        /// </summary>
        /// <remarks>
        /// Included with <see cref="Last"/> in claim <see cref="ClaimTypes.Surname"/> 
        /// </remarks>
        public virtual string Suffix { get; set; }

        /// <summary>
        /// Full Name using <see cref="First"/> <see cref="Middle"/> <see cref="Last"/> <see cref="Suffix"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypes.Name"/>
        /// </remarks>
        public virtual string FullName => System.Text.RegularExpressions.Regex.Replace(($"{First} {Middle} {Last} {Suffix}").Trim(), @"\s+", " ");
    }   
}