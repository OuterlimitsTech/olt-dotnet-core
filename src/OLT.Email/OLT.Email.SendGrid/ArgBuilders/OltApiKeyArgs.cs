using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Email.SendGrid
{
    public abstract class OltApiKeyArgs<T> : OltFromEmailArgs<T>
        where T : OltApiKeyArgs<T>
    {
        protected internal string ApiKey { get; set; }

        protected OltApiKeyArgs()
        {
        }

        /// <summary>
        /// SendGrid API Key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public T WithApiKey(string apiKey)
        {
            this.ApiKey = apiKey;
            return (T)this;
        }

        /// <summary>
        /// Determines if Email can be sent depending on <see cref="Production"/> is true or <see cref="TestWhitelist"/> <see cref="Production"/> is false
        /// </summary>
        public override bool AllowSend(string emailAddress)
        {
            if (base.Enabled)
            {
                return true;
            }

            return Whitelist?.Domain?.Any(p => emailAddress.EndsWith(p, StringComparison.OrdinalIgnoreCase)) == true ||
                   Whitelist?.Email?.Any(p => emailAddress.Equals(p, StringComparison.OrdinalIgnoreCase)) == true;
        }


        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                errors.Add("SendGrid API Key Missing");
            }
            return errors;
        }
    }
}
