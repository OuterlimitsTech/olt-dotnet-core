using OLT.Email.Smtp.Tests.Assets;
using System;
using System.Collections.Generic;
using Xunit;

namespace OLT.Email.Smtp.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void OltSmtpConfiguration()
        {
            var fromEmail = Faker.Internet.Email();
            var fromName = Faker.Name.FullName();

            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();

            var name = Faker.Name.FullName();
            var email = $"{Faker.Internet.UserName()}@{whiteDomain}";
            var bccName = Faker.Name.FullName();
            var bccEmail = Faker.Internet.Email();

            var subject = Faker.Lorem.Sentence();
            var message = Faker.Lorem.Paragraph();


            var smtpEmail = new OltSmtpEmail
            {
                Subject = subject,
                Body = message,
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress
                        {
                            Name = name,
                            Email = email
                        }
                    },
                    CarbonCopy = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress
                        {
                            Name = bccEmail,
                            Email = bccName
                        }
                    }
                },
                From = new OltEmailAddress
                {
                    Name = fromName,
                    Email = fromEmail
                },
            };


            var server = new SmtpServerConfig
            {
                Server = Faker.Internet.DomainName(),
                DisableSsl = true,                
                Port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue)),
                Username = Faker.Internet.UserName(),
                Password = Faker.Lorem.GetFirstWord()
            };

            var config = new OltSmtpConfiguration(server);
            config.TestWhitelist.Domain.Add(whiteDomain);
            config.TestWhitelist.Email.Add(whiteEmail);
            config.From.Name = fromName;
            config.From.Email = fromEmail;

            Assert.NotNull(config.Smtp);
            Assert.Equal(server.Server, config.Smtp.Server);
            Assert.Equal(server.Port, config.Smtp.Port);
            Assert.Equal(server.DisableSsl, config.Smtp.DisableSsl);

            Assert.False(config.Production);
            Assert.Equal(fromName, config.From.Name);
            Assert.Equal(fromEmail, config.From.Email);
            Assert.NotEmpty(config.TestWhitelist.Email);
            Assert.NotEmpty(config.TestWhitelist.Domain);
            Assert.Equal(whiteEmail, config.TestWhitelist.Email[0]);
            Assert.Equal(whiteDomain, config.TestWhitelist.Domain[0]);

            var args = config.BuildArgs(smtpEmail);
            
            Assert.False(args.AllowSend(Faker.Internet.FreeEmail()));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.True(args.AllowSend(email));

            config.Production = true;
            Assert.True(config.Production);

            args = config.BuildArgs(smtpEmail);

            Assert.True(args.AllowSend(Faker.Internet.FreeEmail()));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.True(args.AllowSend(email));

            args = config.BuildArgs(smtpEmail, server);

            Assert.True(args.AllowSend(Faker.Internet.FreeEmail()));
            Assert.True(args.AllowSend(whiteEmail));
            Assert.True(args.AllowSend(email));
        }

        [Fact]
        public void OltSmtpServer()
        {
            var server = Faker.Internet.DomainName();
            var port = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue));
            var username = Faker.Internet.UserName();
            var password = Faker.Lorem.GetFirstWord();

            var model = new SmtpServerConfig();
            model.Server = server;
            model.Port = port;
            model.Username = username;
            model.Password = password;

            Assert.Equal(server, model.Server);
            Assert.Equal(port, model.Port);
            Assert.Equal(username, model.Username);
            Assert.Equal(password, model.Password);
            Assert.False(model.DisableSsl);

            model.DisableSsl = true;

            Assert.True(model.DisableSsl);

            var smtpServer = new OltSmtpServer(model);
            Assert.Equal(server, smtpServer.Server);
            Assert.Equal(port, smtpServer.Port);
            Assert.True(smtpServer.DisableSsl);
        }        

    }
}