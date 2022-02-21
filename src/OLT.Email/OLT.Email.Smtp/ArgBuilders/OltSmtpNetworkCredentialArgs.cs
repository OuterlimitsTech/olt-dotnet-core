using System.Collections.Generic;

namespace OLT.Email.Smtp
{
    public abstract class OltSmtpNetworkCredentialArgs<T> : OltBodyArgs<T>
        where T : OltSmtpNetworkCredentialArgs<T>
    {        
        protected internal string SmtpUsername { get; set; }
        protected internal string SmtpPassword { get; set; }

        protected OltSmtpNetworkCredentialArgs()
        {
        }

        /// <summary>
        /// SMTP Server Username
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public T WithSmtpNetworkCredentials(string username, string password)
        {
            this.SmtpUsername = username;
            this.SmtpPassword = password;
            return (T)this;
        }

        //protected override List<string> Validate()
        //{
        //    var errors = base.Validate();
        //    if (string.IsNullOrWhiteSpace(SmtpUsername))
        //    {
        //        errors.Add("Smtp Username Missing");
        //    }
        //    if (string.IsNullOrWhiteSpace(SmtpPassword))
        //    {
        //        errors.Add("Smtp Password Missing");
        //    }
        //    return errors;
        //}
    }

   
}
