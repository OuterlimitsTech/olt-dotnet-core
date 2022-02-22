using OLT.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email
{
    public class OltSmtpService : OltDisposable
    {
        public OltEmailResult SendEmail(OltSmtpArgs request)
        {
            return new OltEmailResult();
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
    }
}
