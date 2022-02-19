using System.Collections.Generic;

namespace OLT.Email
{
    /// <summary>
    /// Whitelist of top-level domains and emails for sending emails in non-production environments
    /// </summary>
    public class OltEmailConfigurationWhitelist
    {
        /// <summary>
        /// List of top-level domains for sending emails in non-production environments
        /// </summary>
        /// <example>
        /// outerlimitstech.com
        /// </example>
        public List<string> Domain { get; set; } = new List<string>();

        /// <summary>
        /// List of email addresses for sending emails in non-production environments
        /// </summary>
        /// <example>
        /// john.doe@fake-email.com
        /// </example>
        public List<string> Email { get; set; } = new List<string>();
    }
}