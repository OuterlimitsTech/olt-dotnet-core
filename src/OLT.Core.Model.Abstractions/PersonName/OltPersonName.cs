﻿using OLT.Identity.Abstractions;

namespace OLT.Core
{
    public class OltPersonName : IOltPersonName
    {
        public OltPersonName()
        {
            
        }

        public OltPersonName(string? first, string? last) : this()
        {
            First = first;
            Last = last;
        }

        public OltPersonName(string? first, string? middle, string? last, string? suffix) : this(first, last)
        {
            Middle = middle;
            Suffix = suffix;
        }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.GivenName"/>
        /// </remarks>
        public virtual string? First { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.MiddleName"/>
        /// </remarks>
        public virtual string? Middle { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.FamilyName"/>
        /// </remarks>
        public virtual string? Last { get; set; }

        /// <summary>
        /// Name Suffix (Jr, Sr, II, III, IV, V)
        /// </summary>
        /// <remarks>
        /// Included with <see cref="Last"/> in claim <see cref="ClaimTypeNames.FamilyName"/> 
        /// </remarks>
        public virtual string? Suffix { get; set; }

        /// <summary>
        /// Full Name using <see cref="First"/> <see cref="Middle"/> <see cref="Last"/> <see cref="Suffix"/>
        /// </summary>
        /// <remarks>
        /// Claim <see cref="ClaimTypeNames.Name"/>
        /// </remarks>
        public virtual string? FullName => System.Text.RegularExpressions.Regex.Replace(($"{First} {Middle} {Last} {Suffix}").Trim(), @"\s+", " ", System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(100));
    }   
}