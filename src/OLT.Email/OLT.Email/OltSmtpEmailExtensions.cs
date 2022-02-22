
using System;

namespace OLT.Email
{

    public static class OltSmtpEmailExtensions
    {
        public static OltSmtpArgs BuildArgs<T>(this OltSmtpConfiguration configuration, T smtpEmailTemplate)  where T : IOltSmtpEmail
        {
            var args = new OltSmtpArgs()
                .WithFromEmail(smtpEmailTemplate.From)
                .WithWhitelist(configuration.TestWhitelist)
                .WithSmtpHost(configuration.Smtp.Host)                
                .WithBody(smtpEmailTemplate.Body)
                .WithSubject(smtpEmailTemplate.Subject)
                .WithRecipients(smtpEmailTemplate.Recipients)                
                .EnableProductionEnvironment(configuration.Production);

            
            if (configuration.Smtp?.Credentials?.Username != null && configuration.Smtp?.Credentials?.Password != null)
            {
                args.WithSmtpNetworkCredentials(configuration.Smtp.Credentials.Username, configuration.Smtp.Credentials.Password);
            }

            if (configuration.Smtp.Port != null && configuration.Smtp.Port > 0)
            {
                args.WithSmtpPort(configuration.Smtp.Port.Value);
            }

            return args;
        }

        public static OltSmtpArgs BuildArgs<T>(this OltSmtpServer smtpServer, bool disableWhitelist, T smtpEmailTemplate) where T : IOltSmtpEmail
        {
            var args = new OltSmtpArgs()
                .WithFromEmail(smtpEmailTemplate.From)
                .WithSmtpHost(smtpServer.Host)
                .WithBody(smtpEmailTemplate.Body)
                .WithSubject(smtpEmailTemplate.Subject)
                .WithRecipients(smtpEmailTemplate.Recipients)
                .EnableProductionEnvironment(disableWhitelist);


            if (smtpServer.Credentials?.Username != null && smtpServer.Credentials?.Password != null)
            {
                args.WithSmtpNetworkCredentials(smtpServer.Credentials.Username, smtpServer.Credentials.Password);
            }

            if (smtpServer.Port != null && smtpServer.Port > 0)
            {
                args.WithSmtpPort(smtpServer.Port.Value);
            }

            return args;
        }

        /// <summary>
        /// Sends email with exception using SendGrid SMTP server using api key
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="email"></param>
        /// <param name="throwException"></param>
        public static bool OltEmailError(this Exception ex, IOltApplicationErrorEmail email, bool throwException)
        {

            //MailMessage mailMessage = new MailMessage
            //{
            //    From = new MailAddress(email.From.Email, email.From.Name)
            //};

            //email.To.ToList().ForEach(rec => mailMessage.To.Add(new MailAddress(rec.Email, rec.Name)));
            //mailMessage.Body = $"The following error occurred:{Environment.NewLine}{ex}";
            //mailMessage.Subject = $"APPLICATION ERROR on {email.AppName} {email.Environment} Environment occurred at {DateTimeOffset.Now:f}";

            //try
            //{

            //    using (var client = new SmtpClient(email.SmtpConfiguration.Server, email.SmtpConfiguration.Port))
            //    {
            //        if (!email.SmtpConfiguration.DisableSsl)
            //        {
            //            client.EnableSsl = true;
            //        }
            //        client.UseDefaultCredentials = false;
            //        client.Port = email.SmtpConfiguration.Port;
            //        client.Credentials = new NetworkCredential(email.SmtpConfiguration.Username, email.SmtpConfiguration.Password);
            //        client.Send(mailMessage);
            //        return true;
            //    }

            //}
            //catch (Exception mailException)
            //{
            //    Console.Write(mailException);
            //    if (throwException)
            //    {
            //        throw;
            //    }
            //}

            return false;
        }

        //public static OltSendGridTemplateArgs BuildArgs<T>(this OltEmailConfigurationSendGrid configuration, T template)
        //    where T : IOltSmtpEmail
        //{
        //    var args = new OltSendGridTemplateArgs()
        //        .WithFromEmail(configuration.From)
        //        .WithWhitelist(configuration.TestWhitelist)
        //        .WithApiKey(configuration.ApiKey)
        //        .WithTemplate(template)
        //        .IsProduction(configuration.Production);

        //    return args;
        //}

        ///// <summary>
        ///// Configure SMTP email with a whitelist for non-production environments
        ///// </summary>
        ///// <typeparam name="TConfig"><seealso cref="IOltEmailConfiguration"/></typeparam>
        ///// <param name="services"></param>
        ///// <param name="configuration"><seealso cref="IOltEmailConfiguration"/></param>
        ///// <returns></returns>
        //public static IServiceCollection ConfigureOltEmailSmtpWithWhitelist<TConfig>(this IServiceCollection services, TConfig configuration) where TConfig : class, IOltEmailConfiguration
        //{
        //    var args = new OltEmailArgs()
        //        .WithFromEmail(configuration.From.Email, configuration.From.Name)
        //        .WithWhitelist(configuration.TestWhitelist)
        //        .IsProduction(configuration.Production);

        //    services.AddSingleton(args);

        //    return services;

        //}

        ///// <summary>
        ///// Configure SMTP email with a whitelist for non-production environments
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="args"></param>
        ///// <returns></returns>
        //public static IServiceCollection ConfigureOltEmailSmtpWithWhitelist(this IServiceCollection services, OltEmailArgs args)
        //{
        //    services.AddSingleton(args);
        //    return services;
        //}

        /////// <summary>
        /////// Sends email using SendGrid SMTP server using api key
        /////// </summary>
        /////// <param name="email"></param>
        /////// <param name="message">Message to Send</param>
        /////// <param name="throwException">Throw Exception on error</param>
        ////public static bool OltEmail(this IOltSmtpEmail email, string message, bool throwException)
        ////{

        ////    try
        ////    {
        ////        MailMessage mailMessage = new MailMessage
        ////        {
        ////            From = new MailAddress(email.From.Email, email.From.Name)
        ////        };


        ////        email.To.ToList().ForEach(rec => mailMessage.To.Add(new MailAddress(rec.Email, rec.Name)));
        ////        mailMessage.Body = message;
        ////        mailMessage.Subject = email.Subject;

        ////        using (var client = new SmtpClient(email.SmtpConfiguration.Server, email.SmtpConfiguration.Port))
        ////        {
        ////            if (!email.SmtpConfiguration.DisableSsl)
        ////            {
        ////                client.EnableSsl = true;
        ////            }
        ////            client.UseDefaultCredentials = false;
        ////            client.Credentials = new NetworkCredential(email.SmtpConfiguration.Username, email.SmtpConfiguration.Password);
        ////            client.Send(mailMessage);
        ////            return true;
        ////        }
        ////    }
        ////    catch
        ////    {
        ////        if (throwException)
        ////        {
        ////            throw;
        ////        }
        ////    }

        ////    return false;

        ////}

        /////// <summary>
        /////// Sends email with exception using SendGrid SMTP server using api key
        /////// </summary>
        /////// <param name="ex"></param>
        /////// <param name="email"></param>
        /////// <param name="fromAddress"></param>
        /////// <param name="throwException"></param>
        ////public static bool OltEmailError(this Exception ex, IOltApplicationErrorEmail email, bool throwException)
        ////{

        ////    MailMessage mailMessage = new MailMessage
        ////    {
        ////        From = new MailAddress(email.From.Email, email.From.Name)
        ////    };

        ////    email.To.ToList().ForEach(rec => mailMessage.To.Add(new MailAddress(rec.Email, rec.Name)));
        ////    mailMessage.Body = $"The following error occurred:{Environment.NewLine}{ex}";
        ////    mailMessage.Subject = $"{email.AppName} APPLICATION ERROR on {email.Environment} Environment occurred at {DateTimeOffset.Now:f}";

        ////    try
        ////    {

        ////        using (var client = new SmtpClient(email.SmtpConfiguration.Server, email.SmtpConfiguration.Port))
        ////        {
        ////            if (!email.SmtpConfiguration.DisableSsl)
        ////            {
        ////                client.EnableSsl = true;
        ////            }
        ////            client.UseDefaultCredentials = false;
        ////            client.Port = email.SmtpConfiguration.Port;
        ////            client.Credentials = new NetworkCredential(email.SmtpConfiguration.Username, email.SmtpConfiguration.Password);
        ////            client.Send(mailMessage);
        ////            return true;
        ////        }

        ////    }
        ////    catch (Exception mailException)
        ////    {
        ////        Console.Write(mailException);
        ////        if (throwException)
        ////        {
        ////            throw;
        ////        }
        ////    }

        ////    return false;
        ////}
    }
}