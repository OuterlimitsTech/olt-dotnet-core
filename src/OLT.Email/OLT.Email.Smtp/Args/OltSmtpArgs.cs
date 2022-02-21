﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OLT.Email.Smtp
{
    public class OltSmtpArgs : OltSmtpNetworkCredentialArgs<OltSmtpArgs>
    {

        public SmtpClient CreateClient()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltSmtpValidationException(errors);
            }
                
            var client = new SmtpClient(SmtpHost, SmtpPort);

            if (!SmtpDisableSSL)
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
