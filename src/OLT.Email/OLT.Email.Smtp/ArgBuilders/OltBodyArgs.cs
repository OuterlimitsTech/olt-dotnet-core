using System;
using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public abstract class OltBodyArgs<T> : OltSubjectLineArgs<T>
        where T : OltBodyArgs<T>
    {
        protected internal string Body { get; set; }

        protected OltBodyArgs()
        {
        }

        /// <summary>
        /// Email Body
        /// </summary>
        /// <returns></returns>
        public T WithBody(string body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            this.Body = body;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(Body))
            {
                errors.Add(OltSmtpArgErrors.Body);
            }
            return errors;
        }
    }

   
}
