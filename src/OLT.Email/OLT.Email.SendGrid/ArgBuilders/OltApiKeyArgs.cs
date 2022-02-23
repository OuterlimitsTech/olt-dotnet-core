using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OLT.Email.SendGrid
{
    public abstract class OltApiKeyArgs<T> : OltFromEmailArgs<T>, IOltApiKeyArgs<T>, IOltEmailClient<SendGridClient, SendGridMessage>
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
        /// <exception cref="ArgumentNullException"></exception>
        public T WithApiKey(string apiKey)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }
            this.ApiKey = apiKey;
            return (T)this;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                errors.Add(OltArgErrorsSendGrid.ApiKey);
            }
            return errors;
        }

        public SendGridClient CreateClient()
        {
            return new SendGridClient(ApiKey);
        }

        public abstract SendGridMessage CreateMessage(OltEmailRecipientResult recipients);


        public virtual void ConfigureRecipients(SendGridMessage msg, OltEmailRecipientResult recipients)
        {
            recipients.To.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.AddTo(new EmailAddress(rec.Email, rec.Name));
            });

            recipients.CarbonCopy.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.AddCc(new EmailAddress(rec.Email, rec.Name));
            });

        }
    }
}
