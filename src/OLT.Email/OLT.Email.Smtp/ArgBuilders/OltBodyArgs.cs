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
        /// Send Grid Template
        /// </summary>
        /// <returns></returns>
        public T WithBody(string value)
        {
            this.Body = value;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(Body))
            {
                errors.Add("Email Body Missing");
            }
            return errors;
        }
    }

   
}
