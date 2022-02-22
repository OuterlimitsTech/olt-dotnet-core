using System;
using System.Collections.Generic;

namespace OLT.Email
{

    public abstract class OltFromEmailArgs<T> : OltAttachmentsArgs<T>
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
        /// <exception cref="ArgumentNullException"></exception>
        public T WithFromEmail(string email, string name = null)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            this.From = new OltEmailAddress(email, name);
            return (T)this;
        }


        /// <summary>
        /// From Email
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public T WithFromEmail(IOltEmailAddress email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            this.From = new OltEmailAddress(email.Email, email.Name);
            return (T)this;
        }


        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (string.IsNullOrWhiteSpace(From?.Email))
            {
                errors.Add(OltCommonArgErrors.From);
            }

            return errors;
        }
    }

}
