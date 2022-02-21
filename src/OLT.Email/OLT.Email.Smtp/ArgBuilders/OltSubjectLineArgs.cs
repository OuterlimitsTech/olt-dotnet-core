using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public abstract class OltSubjectLineArgs<T> : OltSmtpPortArgs<T>
       where T : OltSubjectLineArgs<T>
    {
        protected internal string SubjectLine { get; set; }

        protected OltSubjectLineArgs()
        {
        }

        /// <summary>
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        public T WithSubject(string value)
        {
            this.SubjectLine = value;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(SubjectLine))
            {
                errors.Add("Email Subject Line Missing");
            }
            return errors;
        }
    }

   
}
