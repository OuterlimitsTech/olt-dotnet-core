using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OLT.Email.SendGrid
{

    public static class OltEmailSendGridExtensions
    {
        public static OltSendGridClient BuildOltEmailClient<T>(this OltEmailConfigurationSendGrid configuration, T template)
            where T : IOltEmailTemplateId, IOltEmailTemplate
        {
            var args = new OltSendGridClient()
                .WithFromEmail(configuration.From)                
                .WithWhitelist(configuration.TestWhitelist)
                .WithApiKey(configuration.ApiKey)
                .WithTemplate(template)
                .WithRecipients(template.Recipients)
                .EnableProductionEnvironment(configuration.Production);

            if (configuration.DisableClickTracking)
            {
                args.WithoutClickTracking();
            }

            return args;
        }


    }
}
