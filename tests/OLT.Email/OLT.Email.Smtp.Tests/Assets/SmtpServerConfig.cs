using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.Smtp.Tests.Assets
{
  
    public class SmtpServerConfig : OltSmtpServer, IOltSmtpServerCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
