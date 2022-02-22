using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OLT.Email
{
    public class OltSmtpArgs : OltSmtpNetworkCredentialArgs<OltSmtpArgs>
    {

        public SmtpClient CreateClient()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltEmailValidationException(errors);
            }
                
            var client = new SmtpClient(SmtpHost, SmtpPort);

            if (!SmtpSSLDisabled)
            {
                client.EnableSsl = true;
            }

            if (!string.IsNullOrWhiteSpace(SmtpUsername) || !string.IsNullOrWhiteSpace(SmtpPassword))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            }

            
            return client;
        }
    }
}
