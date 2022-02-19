using System;
using System.Linq;
using OLT.Core;

namespace OLT.Email
{
    /// <summary>
    /// OLT Email Configuration
    /// </summary>
    public class OltEmailConfiguration : IOltEmailConfiguration
    {

        /// <summary>
        /// Default From Address if not specified in Sned
        /// </summary>
        public virtual OltEmailAddress From { get; set; } = new OltEmailAddress();

        /// <summary>
        /// Test Environment whitelist 
        /// </summary>
        public virtual OltEmailConfigurationWhitelist TestWhitelist { get; set; } = new OltEmailConfigurationWhitelist();


        /// <summary>
        /// Running in Production and send all emails if true
        /// </summary>
        public virtual bool Production { get; set; }

        ///// <summary>
        ///// Determines if Email can be sent depending on <see cref="Production"/> is true or <see cref="TestWhitelist"/> <see cref="Production"/> is false
        ///// </summary>
        //public virtual bool SendEmail(string emailAddress)
        //{
        //    if (Production)
        //    {
        //        return true;
        //    }
            
        //    return TestWhitelist?.Domain?.Any(p => emailAddress.EndsWith(p, StringComparison.OrdinalIgnoreCase)) == true ||
        //           TestWhitelist?.Email?.Any(p => emailAddress.Equals(p, StringComparison.OrdinalIgnoreCase)) == true;
        //}

    }
}