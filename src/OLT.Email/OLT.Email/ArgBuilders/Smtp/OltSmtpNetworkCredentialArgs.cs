using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace OLT.Email
{
    public abstract class OltSmtpNetworkCredentialArgs<T> : OltSmtpSendArgs<T>
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
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            this.SmtpUsername = username;
            this.SmtpPassword = password;
            return (T)this;
        }


        protected override SmtpClient CreateClient()
        {
            var client = base.CreateClient();
            if (!string.IsNullOrWhiteSpace(SmtpUsername) || !string.IsNullOrWhiteSpace(SmtpPassword))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            }
            return client;
        }
    }


}
