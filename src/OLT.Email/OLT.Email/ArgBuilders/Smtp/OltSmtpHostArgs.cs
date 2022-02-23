using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email
{
    public abstract class OltSmtpHostArgs<T> : OltFromEmailArgs<T>
       where T : OltSmtpHostArgs<T>
    {
        protected internal string SmtpHost { get; set; }

        protected OltSmtpHostArgs()
        {
        }

        /// <summary>
        /// SMTP Server Host Address
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public T WithSmtpHost(string host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            this.SmtpHost = host;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(SmtpHost))
            {
                errors.Add(OltSmtpArgErrors.Host);
            }
            return errors;
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
