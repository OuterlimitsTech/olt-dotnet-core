using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using OLT.Email;

// ReSharper disable once CheckNamespace
namespace OLT.Core
{

    public static class OltSmtpEmailExtensions
    {
        /// <summary>
        /// Sends email using SendGrid SMTP server using api key
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message">Message to Send</param>
        /// <param name="throwException">Throw Exception on error</param>
        public static bool OltEmail(this IOltSmtpEmail email, string message, bool throwException)
        {

            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(email.From.Email, email.From.Name)
                };


                email.To.ToList().ForEach(rec => mailMessage.To.Add(new MailAddress(rec.Email, rec.Name)));
                mailMessage.Body = message;
                mailMessage.Subject = email.Subject;

                using (var client = new SmtpClient(email.SmtpConfiguration.Server, email.SmtpConfiguration.Port))
                {
                    if (!email.SmtpConfiguration.DisableSsl)
                    {
                        client.EnableSsl = true;
                    }
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(email.SmtpConfiguration.Username, email.SmtpConfiguration.Password);
                    client.Send(mailMessage);
                    return true;
                }
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }

            return false;

        }

        /// <summary>
        /// Sends email with exception using SendGrid SMTP server using api key
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="email"></param>
        /// <param name="fromAddress"></param>
        /// <param name="throwException"></param>
        public static bool OltEmailError(this Exception ex, IOltApplicationErrorEmail email, bool throwException)
        {

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(email.From.Email, email.From.Name)
            };

            email.To.ToList().ForEach(rec => mailMessage.To.Add(new MailAddress(rec.Email, rec.Name)));
            mailMessage.Body = $"The following error occurred:{Environment.NewLine}{ex}";
            mailMessage.Subject = $"{email.AppName} APPLICATION ERROR on {email.Environment} Environment occurred at {DateTimeOffset.Now:f}";

            try
            {

                using (var client = new SmtpClient(email.SmtpConfiguration.Server, email.SmtpConfiguration.Port))
                {
                    if (!email.SmtpConfiguration.DisableSsl)
                    {
                        client.EnableSsl = true;
                    }
                    client.UseDefaultCredentials = false;
                    client.Port = email.SmtpConfiguration.Port;
                    client.Credentials = new NetworkCredential(email.SmtpConfiguration.Username, email.SmtpConfiguration.Password);
                    client.Send(mailMessage);
                    return true;
                }

            }
            catch (Exception mailException)
            {
                Console.Write(mailException);
                if (throwException)
                {
                    throw;
                }
            }

            return false;
        }
    }
}