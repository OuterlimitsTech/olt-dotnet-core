using System;

namespace OLT.Email.SendGrid
{
    public class OltSmtpServerSendGrid : OltSmtpServer
    {        
        protected OltSmtpServerSendGrid()
        {
            Host = "smtp.sendgrid.net";
            Port = 587;
            Credentials.Username = "apikey";            
        }

        public OltSmtpServerSendGrid(string apiKey) : this()
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            Credentials.Password = apiKey;
        }
    }
}
