using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OLT.Email.Smtp
{
    public abstract class OltSmtpServerArgs<T> : OltFromEmailArgs<T>
       where T : OltSmtpServerArgs<T>
    {
        protected internal string SmtpHost { get; set; }

        protected OltSmtpServerArgs()
        {
        }

        /// <summary>
        /// SMTP Server Address
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T WithSmtpHost(string value)
        {
            this.SmtpHost = value;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(SmtpHost))
            {
                errors.Add("Smtp Host Missing");
            }
            return errors;
        }
    }

   
}
