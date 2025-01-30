using OLT.Identity.Abstractions;

namespace OLT.Core
{
    public interface IOltPersonName
    {
        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.GivenName"/>
        /// </remarks>
        string? First { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.MiddleName"/>
        /// </remarks>
        string? Middle { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.FamilyName"/>
        /// </remarks>
        string? Last { get; set; }

        /// <summary>
        /// Name Suffix (Jr, Sr, II, III, IV, V)
        /// </summary>
        /// <remarks>
        /// Included with <see cref="Last"/> in claim <see cref="ClaimTypeNames.FamilyName"/> 
        /// </remarks>
        string? Suffix { get; set; }

        /// <summary>
        /// Full Name using <see cref="First"/> <see cref="Middle"/> <see cref="Last"/> <see cref="Suffix"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.Name"/>
        /// </remarks>
        string? FullName { get; }
    }
}