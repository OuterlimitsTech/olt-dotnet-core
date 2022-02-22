////using Microsoft.Extensions.Configuration;
////using Microsoft.Extensions.Options;
////using OLT.Core;
////using OLT.Email.Smtp.Tests.Assets;
////using System;
////using System.Collections.Generic;
////using Xunit;

////namespace OLT.Email.Smtp.Tests.Smtp
////{

////    public class SendTests
////    {
////        private readonly SmtpServerConfig _smtpConfiguration;
////        private readonly IConfiguration _configuration;

////        public SendTests(
////            IConfiguration configuration,
////            IOptions<SmtpServerConfig> smtpConfiguration)
////        {
////            _smtpConfiguration = smtpConfiguration.Value;
////            _configuration = configuration;
////        }

////        //[Fact]
////        //public void SendEmail()
////        //{
////        //    Assert.NotNull(_smtpConfiguration);
////        //    Assert.NotNull(_smtpConfiguration.Host);
////        //    Assert.True(_smtpConfiguration.Port > 0);
////        //    Assert.NotNull(_smtpConfiguration.Username);
////        //    Assert.NotNull(_smtpConfiguration.Password);
////        //    Assert.False(_smtpConfiguration.DisableSsl);

////        //    var buildVersion = _configuration.GetValue<string>("GITHUB_RUN_NUMBER") ??
////        //                       Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") ??
////        //                       "[No Run Number]";

////        //    var now = DateTimeOffset.Now;
////        //    var name = Faker.Name.FullName();
////        //    var email = _configuration.GetValue<string>("SMTP_TO_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_TO_ADDRESS");
////        //    var bccName = Faker.Name.FullName();
////        //    var bccEmail = Faker.Internet.Email();

////        //    var fromName = $"{Faker.Name.First()} Unit Test";
////        //    var fromEmail = _configuration.GetValue<string>("SMTP_FROM_ADDRESS") ?? Environment.GetEnvironmentVariable("SMTP_FROM_ADDRESS");
////        //    var subject = $"Unit Test Email {now:f} Run - {buildVersion} ";
////        //    var message = $"{buildVersion} -> This was generated on {now:f} from {this.GetType().Assembly.GetName().Name}";


////        //    var smtpEmail = new OltSmtpEmail
////        //    {
////        //        Subject = subject,
////        //        Body = message,
////        //        Recipients = new OltEmailRecipients
////        //        {
////        //            To = new List<IOltEmailAddress>
////        //            {
////        //                new OltEmailAddress
////        //                {
////        //                    Name = name,
////        //                    Email = email
////        //                }
////        //            },
////        //            CarbonCopy = new List<IOltEmailAddress>
////        //            {
////        //                new OltEmailAddress
////        //                {
////        //                    Name = bccName,
////        //                    Email = bccEmail
////        //                }
////        //            }
////        //        },
////        //        From = new OltEmailAddress
////        //        {
////        //            Name = fromName,
////        //            Email = fromEmail
////        //        },
////        //    };


////        //    Assert.NotNull(smtpEmail.Recipients);
////        //    Assert.NotEmpty(smtpEmail.Recipients.To);
////        //    Assert.NotEmpty(smtpEmail.Recipients.CarbonCopy);
////        //    Assert.Equal(name, smtpEmail.Recipients.To[0].Name);
////        //    Assert.Equal(email, smtpEmail.Recipients.To[0].Email);
////        //    Assert.Equal(bccName, smtpEmail.Recipients.CarbonCopy[0].Name);
////        //    Assert.Equal(bccEmail, smtpEmail.Recipients.CarbonCopy[0].Email);
////        //    Assert.Equal(subject, smtpEmail.Subject);
////        //    Assert.Equal(message, smtpEmail.Body);
////        //    Assert.Equal(fromName, smtpEmail.From.Name);
////        //    Assert.Equal(fromEmail, smtpEmail.From.Email);

            
////        //    //using(var smtpService = new OltSmtpService())
////        //    //{
////        //    //    var results = smtpService.SendEmail(smtpEmail);
////        //    //}
            
////        //    //Assert.True(smtpEmail.OltEmail(message, true));

////        //    //var invalidSmtpEmail = new OltSmtpEmail
////        //    //{
////        //    //    Subject = subject,
////        //    //    To = new List<IOltEmailAddress>
////        //    //    {
////        //    //        new OltEmailAddress
////        //    //        {
////        //    //            Name = name,
////        //    //            Email = email
////        //    //        }
////        //    //    },
////        //    //    From = new OltEmailAddress
////        //    //    {
////        //    //        Name = fromName,
////        //    //        Email = fromEmail                
////        //    //    },
////        //    //    SmtpConfiguration = new OltSmtpConfiguration()
////        //    //};

////        //    //Assert.Throws<System.InvalidOperationException>(() => invalidSmtpEmail.OltEmail(message, true));
////        //    //Assert.False(invalidSmtpEmail.OltEmail(message, false));

////        //}
////    }
////}