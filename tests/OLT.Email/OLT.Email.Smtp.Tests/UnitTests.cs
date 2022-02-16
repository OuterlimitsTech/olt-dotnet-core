using Microsoft.Extensions.Configuration;
using OLT.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.Email.Smtp.Tests
{

    public class SendTests
    {
        private readonly IOltSmtpConfiguration _smtpConfiguration;
        private readonly IConfiguration _configuration;

        public SendTests(
            IConfiguration configuration,
            IOltSmtpConfiguration smtpConfiguration)            
        {
            _smtpConfiguration = smtpConfiguration;
            _configuration = configuration;
        }

        [Fact]
        public void SendEmail()
        {
            Assert.NotNull(_smtpConfiguration);
            Assert.NotNull(_smtpConfiguration.Server);
            Assert.True(_smtpConfiguration.Port > 0);
            Assert.NotNull(_smtpConfiguration.Username);
            Assert.NotNull(_smtpConfiguration.Password);
            Assert.False(_smtpConfiguration.DisableSsl);

            var buildVersion = _configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??
                               Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??
                               "[No Run Number]";

            var now = DateTimeOffset.Now;
            var name = Faker.Name.FullName();
            var email = _configuration.GetValue<string>("SMTP_TO_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_TO_ADDRESS");

            var fromName = $"{Faker.Name.First()} Unit Test";
            var fromEmail = _configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
            var subject = $"Unit Test Email {now:f} Run - {buildVersion} ";
            var message = $"{buildVersion} -> This was generated on {now:f} from {this.GetType().Assembly.GetName().Name}";

            var smtpEmail = new OltSmtpEmail
            {
                Subject = subject,
                To = new List<IOltEmailAddress>
                {
                    new OltEmailAddress
                    {
                        Name = name,
                        Email = email
                    }
                },
                From = new OltEmailAddress
                {
                    Name = fromName,
                    Email = fromEmail
                },
                SmtpConfiguration = _smtpConfiguration
            };


            Assert.NotEmpty(smtpEmail.To);
            Assert.Equal(name, smtpEmail.To[0].Name);
            Assert.Equal(email, smtpEmail.To[0].Email);
            Assert.Equal(subject, smtpEmail.Subject);
            Assert.Equal(fromName, smtpEmail.From.Name);
            Assert.Equal(fromEmail, smtpEmail.From.Email);


            Assert.True(smtpEmail.OltEmail(message, true));

            var invalidSmtpEmail = new OltSmtpEmail
            {
                Subject = subject,
                To = new List<IOltEmailAddress>
                {
                    new OltEmailAddress
                    {
                        Name = name,
                        Email = email
                    }
                },
                From = new OltEmailAddress
                {
                    Name = fromName,
                    Email = fromEmail                
                },
                SmtpConfiguration = new OltSmtpConfiguration()
            };

            Assert.Throws<System.InvalidOperationException>(() => invalidSmtpEmail.OltEmail(message, true));
            Assert.False(invalidSmtpEmail.OltEmail(message, false));

        }
    }
}