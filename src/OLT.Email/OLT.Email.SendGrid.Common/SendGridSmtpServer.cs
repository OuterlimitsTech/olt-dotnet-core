using System;

namespace OLT.Email.SendGrid
{
    public class OltSendGridSmtpServer : OltSmtpServer
    {        
        public OltSendGridSmtpServer()
        {
            Host = "smtp.sendgrid.net";
            Port = 587;
            Credentials.Username = "apiKey";
            Credentials.Password =  Environment.GetEnvironmentVariable("SMTP_PASSWORD");
        }

        public OltSendGridSmtpServer(string apiKey) : this()
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            Credentials.Password = apiKey;
        }
    }
}
