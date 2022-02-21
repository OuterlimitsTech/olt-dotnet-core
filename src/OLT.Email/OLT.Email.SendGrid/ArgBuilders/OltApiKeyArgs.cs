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
