using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OLT.Email.SendGrid
{
    public class OltSendGridEmailService : OltEmailServiceBase
    {

        //protected virtual SendGridMessage CreateMessage(OltEmailResult result, OltEmailAddress from)
        //{
        //    var msg = new SendGridMessage();
        //    msg.SetFrom(new EmailAddress(from.Email, from.Name));
        //    ConfigureRecipients(msg, result.RecipientResults);
        //    return msg;
        //}

        //protected virtual OltEmailResult CreateResult(OltSendGridArgs args, IOltEmailRequest request, OltEmailAddress from)
        //{
        //    var result = new OltEmailResult
        //    {
        //        Errors = Validate(request, from),
        //        RecipientResults = new OltEmailRecipientResult(request.Recipients, args)
        //    };

        //    if (string.IsNullOrWhiteSpace(args.ApiKey))
        //    {
        //        result.Errors.Add("SendGrid API Key Missing");
        //    }

        //    return result;
        //}

        //protected virtual OltEmailResult CreateResult(IOltEmailTemplateRequest request, OltEmailAddress from)
        //{
        //    var result = new OltEmailResult
        //    {
        //        Errors = Validate(request, from),
        //        RecipientResults = new OltEmailRecipientResult(request.Recipients, Configuration)
        //    };

        //    if (string.IsNullOrWhiteSpace(Configuration.ApiKey))
        //    {
        //        result.Errors.Add("SendGrid API Key Missing");
        //    }

        //    return result;
        //}



        public virtual OltEmailResult Send(OltSendGridTemplateArgs args) //where T : IOltEmailTemplateRequestSendGrid
        {
            try
            {
                var msg = args.BuildMessage();
                return Send(msg, args);
            }
            catch (OltSendGridValidationException ex)
            {
                return ex.ToEmailResult();
            }            
        }

        protected virtual OltEmailResult Send(SendGridMessage msg, OltSendGridTemplateArgs args)
        {
            var result = new OltEmailResult();
            var client = args.CreateClient();
            var sendResponse = client.SendEmailAsync(msg).Result;
            if (sendResponse.StatusCode != HttpStatusCode.Accepted)
            {
                result.Errors.Add($"{sendResponse.StatusCode.ToString()} - {sendResponse.Body.ReadAsStringAsync().Result}");
            }

            return result;
        }


        //public override OltEmailResult SendEmail(IOltEmailCalendarRequest request, IOltSmtpConfiguration smtpConfiguration, OltEmailAddress @from)
        //{
        //    var result = CreateResult(request, from);

        //    if (!SendEmail(result))
        //    {
        //        return result;
        //    }


        //    using (var smtp = new System.Net.Mail.SmtpClient(smtpConfiguration.Server, smtpConfiguration.Port))
        //    {
        //        if (!smtpConfiguration.DisableSsl)
        //        {
        //            smtp.EnableSsl = true;
        //        }
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential(smtpConfiguration.Username, smtpConfiguration.Password);

        //        var msg = new System.Net.Mail.MailMessage
        //        {
        //            From = new MailAddress(from.Email, from.Name),
        //            Subject = request.Subject,
        //            Body = request.Body
        //        };

        //        request.Recipients.To.ToList().ForEach(rec =>
        //        {
        //            msg.To.Add(new MailAddress(rec.Email, rec.Name));
        //        });

        //        request.Recipients.CarbonCopy.ToList().ForEach(rec =>
        //        {
        //            msg.To.Add(new MailAddress(rec.Email, rec.Name));
        //        });

        //        System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(OltEmailCalendarAttachment.DefaultTextCalendar);
        //        contentType.Parameters.Add("method", "REQUEST");
        //        msg.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
        //        contentType.Parameters.Add("name", OltEmailCalendarAttachment.DefaultFileName);
        //        AlternateView avCal = AlternateView.CreateAlternateViewFromString(Encoding.UTF8.GetString(request.CalendarInvite.Bytes, 0, request.CalendarInvite.Bytes.Length), contentType);
        //        msg.AlternateViews.Add(avCal);

        //        try
        //        {
        //            smtp.Send(msg);
        //        }
        //        catch (Exception exception)
        //        {
        //            result.Errors.Add($"Error - {exception}");
        //        }
        //        finally
        //        {
        //            msg.Dispose();
        //        }
        //    }

        //    return result;
        //}

        //protected virtual OltEmailResult Send(SendGridMessage msg, OltEmailResult result)
        //{
        //    var client = new SendGridClient(Configuration.ApiKey);
        //    var sendResponse = client.SendEmailAsync(msg).Result;
        //    if (sendResponse.StatusCode != HttpStatusCode.Accepted)
        //    {
        //        result.Errors.Add($"{sendResponse.StatusCode.ToString()} - {sendResponse.Body.ReadAsStringAsync().Result}");
        //    }

        //    return result;
        //}





        //protected virtual void ConfigureRecipients(SendGridMessage msg, OltEmailRecipientResult recipients)
        //{
        //    recipients.To?.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
        //    {
        //        msg.AddTo(new EmailAddress(rec.Email, rec.Name));
        //    });

        //    recipients.CarbonCopy?.Where(p => !p.Skipped && string.IsNullOrWhiteSpace(p.Error)).ToList().ForEach(rec =>
        //    {
        //        msg.AddCc(new EmailAddress(rec.Email, rec.Name));
        //    });

        //}

    }
}
