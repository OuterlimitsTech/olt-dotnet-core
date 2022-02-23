using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OLT.Email
{
   
    public class OltEmailClientSmtp : OltSmtpNetworkCredentialArgs<OltEmailClientSmtp>
    {

        public override OltEmailResult Send()
        {
            try
            {
                return Task.Run(() => SendAsync()).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }            
        }

        public override async Task<OltEmailResult> SendAsync()
        {
            var errors = Validate();
            if (errors.Any())
            {
                throw new OltEmailValidationException(errors);
            }

            var result = new OltEmailResult
            {
                RecipientResults = BuildRecipients()
            };
         
            try
            {
                using (var client = CreateClient())
                {
                    using (var msg = CreateMessage(result.RecipientResults))
                    {
                        if (msg.To.Any())
                        {
                            await client.SendMailAsync(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.ToString());
            }

            return result;
        }

       
    }
}
