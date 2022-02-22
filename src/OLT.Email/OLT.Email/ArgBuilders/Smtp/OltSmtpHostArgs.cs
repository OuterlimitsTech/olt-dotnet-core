using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }

   
}
