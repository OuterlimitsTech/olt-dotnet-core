using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email.SendGrid
{

    public static class OltEmailSendGridExtensions
    {
        public static OltSendGridTemplateArgs BuildArgs<T>(this OltEmailConfigurationSendGrid configuration, T template)
            where T : IOltEmailTemplate
        {
            var args = new OltSendGridTemplateArgs()
                .WithFromEmail(configuration.From)
                .WithWhitelist(configuration.TestWhitelist)
                .WithApiKey(configuration.ApiKey)
                .WithTemplate(template)
                .EnableProductionEnvironment(configuration.Production);

            if (configuration.DisableClickTracking)
            {
                args.WithoutClickTracking();
            }

            return args;
        }

        /// <summary>
        /// Sends email with exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="server"></param>
        /// <param name="rethrowException"></param>
        public static void OltEmailError(this Exception ex, string apiKey, IOltApplicationErrorEmail template, bool rethrowException = false)
        {
            //BuildOltEmailClient(ex, server, template).Send();

            if (rethrowException)
            {
                throw ex;
            }
        }
    }
}
