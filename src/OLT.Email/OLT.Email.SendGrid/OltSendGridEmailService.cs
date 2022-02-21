using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OLT.Core;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OLT.Email.SendGrid
{
    
    public class OltSendGridEmailService : OltDisposable
    {
        public virtual async Task<OltEmailResult> SendAsync(OltSendGridTemplateArgs args)
        {
            try
            {
                var msg = args.BuildMessage();
                return await SendAsync(msg, args);
            }
            catch (OltSendGridValidationException ex)
            {
                return ex.ToEmailResult();
            }            
        }

        protected virtual async Task<OltEmailResult> SendAsync(SendGridMessage msg, OltSendGridTemplateArgs args)
        {
            var result = new OltEmailResult();            
            var client = args.CreateClient();
            var sendResponse = await client.SendEmailAsync(msg);
            if (sendResponse.StatusCode != HttpStatusCode.Accepted)
            {
                result.Errors.Add($"{sendResponse.StatusCode.ToString()} - {sendResponse.Body.ReadAsStringAsync().Result}");
            }

            return result;
        }

    }
}
