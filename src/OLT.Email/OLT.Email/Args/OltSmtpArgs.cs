using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OLT.Email
{
    public class OltSmtpArgs : OltSmtpNetworkCredentialArgs<OltSmtpArgs>
    {

        public virtual OltEmailResult Send()
        {
            try
            {
                return Task.Run(() => SendAsync()).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }            
        }

        public virtual async Task<OltEmailResult> SendAsync()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltEmailValidationException(errors);
            }

            var result = new OltEmailResult
            {
                RecipientResults = BuildRecipients()
            };
         
            try
            {
                using (var client = CreateClient())
                {
                    using (var msg = CreateMessage(result.RecipientResults))
                    {
                        if (msg.To.Any())
                        {
                            await client.SendMailAsync(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.ToString());
            }

            return result;
        }

        protected virtual SmtpClient CreateClient()
        {
            var client = new SmtpClient(SmtpHost, SmtpPort);

            if (!SmtpSSLDisabled)
            {
                client.EnableSsl = true;
            }
                        
            if (!string.IsNullOrWhiteSpace(SmtpUsername) || !string.IsNullOrWhiteSpace(SmtpPassword))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            }
            
            return client;
        }

        protected virtual MailMessage CreateMessage(OltEmailRecipientResult recipients)
        {
            var msg = new MailMessage
            {
                From = new MailAddress(From.Email, From.Name),
                Subject = SubjectLine,
                Body = Body,
            };
                        
            ConfigureRecipients(msg, recipients);
            
            return msg;
        }

        protected virtual void ConfigureRecipients(MailMessage msg, OltEmailRecipientResult recipients)
        {
            recipients.To?.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.To.Add(new MailAddress(rec.Email, rec.Name));
            });

            recipients.CarbonCopy?.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
            {
                msg.CC.Add(new MailAddress(rec.Email, rec.Name));
            });            
        }
    }
}
