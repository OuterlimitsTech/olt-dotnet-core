using System;
using System.Linq;
using System.Threading.Tasks;

namespace OLT.Email
{
    public abstract class OltSmtpSendArgs<T> : OltCalendarInviteArgs<T>
      where T : OltSmtpSendArgs<T>
    {
        public override async Task<OltEmailResult> SendAsync()
        {
            var errors = ValidationErrors();
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
