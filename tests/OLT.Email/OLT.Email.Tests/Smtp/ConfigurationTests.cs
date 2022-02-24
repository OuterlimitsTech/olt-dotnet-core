using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    public class SmtpConfigurationTests
    {
        public class AppSettingsJsonDto
        {
            public OltSmtpConfiguration SmtpEmailConfig { get; set; } = new OltSmtpConfiguration();
        }


        private readonly OltEmailConfiguration _emailConfiguration;
        private readonly string FakeHost = Faker.Internet.DomainName();

        public SmtpConfigurationTests(
            IOptions<OltSmtpConfiguration> options)
        {
            _emailConfiguration = options.Value;
        }

        //[Fact]
        //public void AllowSend()
        //{
        //    var whiteEmail = Faker.Internet.Email();
        //    var whiteDomain = Faker.Internet.DomainName();
        //    var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";
        //    var badDomainEmail = $"{Faker.Internet.UserName()}@{Faker.Internet.DomainName()}";

        //    var emailList = new List<string>
        //    {
        //        whiteEmail,
        //        Faker.Internet.Email(),
        //        Faker.Internet.FreeEmail(),
        //        Faker.Internet.Email(),
        //        Faker.Internet.FreeEmail(),
        //    };

        //    var domainList = new List<string>
        //    {
        //        whiteDomain,
        //        Faker.Internet.DomainName(),
        //        Faker.Internet.DomainName(),
        //        Faker.Internet.DomainName()
        //    };

        //    var smtpServer = new OltSmtpServer
        //    {
        //        Host = FakeHost,
        //        Credentials = null
        //    };

        //    var smtpEmail = SmtpHelper.FakerSmtpEmail(7, 5);

        //    var args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, false, smtpEmail).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = null });
        //    Assert.False(args.AllowSend(Faker.Internet.Email()));
        //    Assert.False(args.AllowSend(badDomainEmail));
        //    Assert.False(args.AllowSend(whiteEmail));
        //    Assert.False(args.AllowSend(whiteDomainEmail));

        //    args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, false, smtpEmail).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = emailList });
        //    Assert.False(args.AllowSend(Faker.Internet.Email()));
        //    Assert.False(args.AllowSend(badDomainEmail));
        //    Assert.True(args.AllowSend(whiteEmail));
        //    Assert.False(args.AllowSend(whiteDomainEmail));

        //    args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, false, smtpEmail).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = domainList, Email = null });
        //    Assert.False(args.AllowSend(Faker.Internet.Email()));
        //    Assert.False(args.AllowSend(badDomainEmail));
        //    Assert.False(args.AllowSend(whiteEmail));
        //    Assert.True(args.AllowSend(whiteDomainEmail));

        //    args = OltSmtpEmailExtensions.BuildOltEmailClient(smtpServer, false, smtpEmail).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = domainList, Email = emailList });
        //    Assert.False(args.AllowSend(Faker.Internet.Email()));
        //    Assert.False(args.AllowSend(badDomainEmail));
        //    Assert.True(args.AllowSend(whiteEmail));
        //    Assert.True(args.AllowSend(whiteDomainEmail));

        //    new OltEmailClientSmtp().EnableProductionEnvironment(false).Invoking(args => args.WithWhitelist(new OltEmailAddress { Email = null })).Should().Throw<InvalidOperationException>();
        //}

        //[Fact]
        //public void ConfigAllowSend()
        //{
        //    var fromEmail = Faker.Internet.Email();
        //    var fromName = Faker.Name.FullName();

        //    var whiteEmail = Faker.Internet.Email();
        //    var whiteDomain = Faker.Internet.DomainName();

        //    var name = Faker.Name.FullName();
        //    var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";

        //    var bccName = Faker.Name.FullName();
        //    var bccEmail = Faker.Internet.Email();

        //    var subject = Faker.Lorem.Sentence();
        //    var message = Faker.Lorem.Paragraph();



        //    var server = new OltSmtpServer
        //    {
        //        Host = Faker.Internet.DomainName(),
        //        DisableSsl = true,                
        //        Port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue)),
        //        Credentials = new OltSmtpCredentials
        //        {
        //            Username = Faker.Internet.UserName(),
        //            Password = Faker.Lorem.GetFirstWord()
        //        }
        //    };

        //    var config = new OltSmtpConfiguration
        //    {
        //        Smtp = server
        //    };

        //    config.TestWhitelist.Domain.Add(whiteDomain);
        //    config.TestWhitelist.Email.Add(whiteEmail);
        //    config.From.Name = fromName;
        //    config.From.Email = fromEmail;

        //    Assert.NotNull(config.Smtp);
        //    Assert.Equal(server.Host, config.Smtp.Host);
        //    Assert.Equal(server.Port, config.Smtp.Port);
        //    Assert.Equal(server.DisableSsl, config.Smtp.DisableSsl);

        //    Assert.False(config.Production);
        //    Assert.Equal(fromName, config.From.Name);
        //    Assert.Equal(fromEmail, config.From.Email);
        //    Assert.NotEmpty(config.TestWhitelist.Email);
        //    Assert.NotEmpty(config.TestWhitelist.Domain);
        //    Assert.Equal(whiteEmail, config.TestWhitelist.Email[0]);
        //    Assert.Equal(whiteDomain, config.TestWhitelist.Domain[0]);



        //    var smtpEmail = new OltSmtpEmail
        //    {
        //        Subject = subject,
        //        Body = message,
        //        Recipients = new OltEmailRecipients
        //        {
        //            To = new List<IOltEmailAddress>
        //            {
        //                new OltEmailAddress
        //                {
        //                    Name = name,
        //                    Email = whiteDomainEmail
        //                }
        //            },
        //            CarbonCopy = new List<IOltEmailAddress>
        //            {
        //                new OltEmailAddress
        //                {
        //                    Name = bccName,
        //                    Email = bccEmail,
        //                }
        //            }
        //        },
        //        From = new OltEmailAddress
        //        {
        //            Name = fromName,
        //            Email = fromEmail
        //        },
        //    };


        //    var args = OltSmtpEmailExtensions.BuildOltEmailClient(config, smtpEmail);
            
        //    Assert.False(args.AllowSend(Faker.Internet.FreeEmail()));
        //    Assert.True(args.AllowSend(whiteEmail));
        //    Assert.True(args.AllowSend(whiteDomainEmail));
        //    Assert.True(args.AllowSend($"{Faker.Lorem.GetFirstWord()}@{whiteDomain}"));
            
        //    config.Production = true;
        //    Assert.True(config.Production);

        //    args = OltSmtpEmailExtensions.BuildOltEmailClient(config, smtpEmail);

        //    Assert.True(args.AllowSend(Faker.Internet.FreeEmail()));
        //    Assert.True(args.AllowSend(whiteEmail));
        //    Assert.True(args.AllowSend(whiteDomainEmail));

        //    args = OltSmtpEmailExtensions.BuildOltEmailClient(config, smtpEmail);

        //    Assert.True(args.AllowSend(Faker.Internet.FreeEmail()));
        //    Assert.True(args.AllowSend(whiteEmail));
        //    Assert.True(args.AllowSend(whiteDomainEmail));
        //}

        [Fact]
        public void OltSmtpServer()
        {
            var host = Faker.Internet.DomainName();
            var port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue));
            var username = Faker.Internet.UserName();
            var password = Faker.Lorem.GetFirstWord();

            var server = new OltSmtpServer();

            Assert.NotNull(server.Credentials);
            Assert.Null(server.Port);
            Assert.False(server.DisableSsl);

            server.Host = host;
            server.Port = port;
            server.DisableSsl = true;
            server.Credentials.Username = username;
            server.Credentials.Password = password;

            Assert.Equal(host, server.Host);
            Assert.Equal(port, server.Port);
            Assert.True(server.DisableSsl);
            Assert.Equal(username, server.Credentials.Username);
            Assert.Equal(password, server.Credentials.Password);
        }

        [Fact]
        public async Task Options()
        {
            Assert.NotNull(_emailConfiguration);

            string fileName = "appsettings.json";
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (FileStream openStream = File.OpenRead(filePath))
            {
                AppSettingsJsonDto expectedConfig = await JsonSerializer.DeserializeAsync<AppSettingsJsonDto>(openStream);
                Assert.NotNull(expectedConfig);
                _emailConfiguration.Should().BeEquivalentTo(expectedConfig?.SmtpEmailConfig);
            }

        }

    }
}