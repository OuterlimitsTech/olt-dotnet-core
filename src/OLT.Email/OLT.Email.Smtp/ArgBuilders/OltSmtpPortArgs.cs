using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public abstract class OltSmtpPortArgs<T> : OltSmtpDisableSslArgs<T>
        where T : OltSmtpPortArgs<T>
    {
        protected internal short SmtpPort { get; set; } = 587;

        protected OltSmtpPortArgs()
        {
        }

        /// <summary>
        /// SMTP Server Port
        /// </summary>
        /// <remarks>Default is 587</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public T WithSmtpPort(short value)
        {
            this.SmtpPort = value;
            return (T)this;
        }

        protected override List<string> Validate()
        {
            var errors = base.Validate();
            if (SmtpPort > 0)
            {
                errors.Add("Smtp Port Missing");
            }
            return errors;
        }
    }

   
}
