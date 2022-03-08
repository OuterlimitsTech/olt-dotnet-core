using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email
{
    public abstract class OltSmtpHostArgs<T> : OltFromEmailArgs<T>, IOltSmtpClient
       where T : OltSmtpHostArgs<T>
    {
        protected  string SmtpHost { get; set; }

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
            this.SmtpHost = host ?? throw new ArgumentNullException(nameof(host));
            return (T)this;
        }

        public override List<string> ValidationErrors()
        {
            var errors = base.ValidationErrors();
            if (string.IsNullOrWhiteSpace(SmtpHost))
            {
                errors.Add(OltSmtpArgErrors.Host);
            }
            return errors;
        }

        protected virtual void ConfigureRecipients(MailMessage msg, OltEmailRecipientResult recipients)
        {
            recipients.To.Where(p => !p.Skipped).ToList().ForEach(rec =>
            {
                msg.To.Add(new MailAddress(rec.Email, rec.Name));
            });

            recipients.CarbonCopy.Where(p => !p.Skipped).ToList().ForEach(rec =>
            {
                msg.CC.Add(new MailAddress(rec.Email, rec.Name));
            });
        }

        public abstract SmtpClient CreateClient();
        public abstract MailMessage CreateMessage(OltEmailRecipientResult recipients);

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

        public abstract Task<OltEmailResult> SendAsync();
    }

   
}
