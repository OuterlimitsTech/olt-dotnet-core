using OLT.Constants;

namespace OLT.Core
{
    public interface IOltPersonName
    {
        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.GivenName"/>
        /// </remarks>
        string? First { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.MiddleName"/>
        /// </remarks>
        string? Middle { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.FamilyName"/>
        /// </remarks>
        string? Last { get; set; }

        /// <summary>
        /// Name Suffix (Jr, Sr, II, III, IV, V)
        /// </summary>
        /// <remarks>
        /// Included with <see cref="Last"/> in claim <see cref="OltClaimTypes.FamilyName"/> 
        /// </remarks>
        string? Suffix { get; set; }

        /// <summary>
        /// Full Name using <see cref="First"/> <see cref="Middle"/> <see cref="Last"/> <see cref="Suffix"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="OltClaimTypes.Name"/>
        /// </remarks>
        string? FullName { get; }
    }
}