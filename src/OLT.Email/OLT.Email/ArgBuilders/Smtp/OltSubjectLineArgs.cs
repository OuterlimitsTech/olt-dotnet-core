using System;
using System.Collections.Generic;

namespace OLT.Email
{
    public abstract class OltSubjectLineArgs<T> : OltSmtpPortArgs<T>
       where T : OltSubjectLineArgs<T>
    {
        protected internal string SubjectLine { get; set; }

        protected OltSubjectLineArgs()
        {
        }

        /// <summary>
        /// Email Subject
        /// </summary>
        /// <returns></returns>
        public T WithSubject(string subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }
            this.SubjectLine = subject;
            return (T)this;
        }       

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(SubjectLine))
            {
                errors.Add(OltSmtpArgErrors.Subject);
            }
            return errors;
        }

    }

   
}
