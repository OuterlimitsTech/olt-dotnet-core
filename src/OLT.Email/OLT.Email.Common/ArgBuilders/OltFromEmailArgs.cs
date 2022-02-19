using System.Collections.Generic;

namespace OLT.Email
{

    public abstract class OltFromEmailArgs<T> : OltRecipientsArgs<T>
        where T : OltFromEmailArgs<T>
    {
        protected internal OltEmailAddress From { get; set; }

        protected OltFromEmailArgs()
        {
        }


        /// <summary>
        /// From Email Address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T WithFromEmail(string email, string name = null)
        {
            this.From = new OltEmailAddress(email, name);
            return (T)this;
        }

        /// <summary>
        /// From Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public T WithFromEmail(IOltEmailAddress email)
        {
            this.From = new OltEmailAddress(email.Email, email.Name);
            return (T)this;
        }


        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(From?.Email))
            {
                errors.Add("From Email Missing");
            }

            return errors;
        }
    }

}
