using FluentAssertions;
using OLT.Email.SendGrid.Tests.Assets.SendGrid.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class GeneralTests
    {
        [Fact]
        public void ServerValues()
        {
            var server = new OltSendGridSmtpServer();
            short port = 587;

            Assert.Equal("smtp.sendgrid.net", server.Host);
            Assert.Equal(port, server.Port);
            Assert.False(server.DisableSsl);
            Assert.Equal("apiKey", server.Credentials.Username);
            Assert.Equal(Environment.GetEnvironmentVariable("SMTP_PASSWORD"), server.Credentials.Password);


            var password = Faker.Internet.UserName();
            server = new OltSendGridSmtpServer(password);
            Assert.Equal(password, server.Credentials.Password);

        }


        [Fact]
        public void SendGridValidationExceptionTests()
        {
            var error1 = Faker.Internet.UserName();
            var error2 = Faker.Internet.DomainName();

            var errors = new List<string>() { error1, error2 };
            var ex = new OltSendGridValidationException(errors);
            
            Assert.Equal("SendGrid Validation Errors", ex.Message);
            Assert.NotEmpty(ex.Errors);
            ex.Errors.Should().BeEquivalentTo(errors);

        }

        [Fact]
        public void EmailTemplateTests()
        {
            
            var data = new EmailDataJson
            {
                Build = new EmailDataBuildVersionJson
                {
                    Version = Faker.Country.Name()
                },
                Communication = new EmailDataCommunicationJson
                {
                    Body1 = Faker.Lorem.Sentences(4).LastOrDefault(),
                    Body2 = Faker.Lorem.Sentences(10).LastOrDefault(),
                }
            };

            var template = new JsonEmailTemplate
            {
                TemplateData = data
            };

            var firstName1 = Faker.Name.First();
            var email1 = Faker.Internet.Email();

            var firstName2 = Faker.Name.First();
            var email2 = Faker.Internet.Email();

            var list = new List<OltEmailAddress>
            {
                new OltEmailAddress(email1, firstName1),
                new OltEmailAddress(email2, firstName2)
            };

            template.To.Add(new OltEmailAddress(email1, firstName1));
            template.To.Add(new OltEmailAddress(email2, firstName2));
            template.To.Should().BeEquivalentTo(list);
            template.TemplateData.Should().BeEquivalentTo(data);
            Assert.Equal(nameof(JsonEmailTemplate), template.TemplateId);
            template.GetTemplateData().Should().BeEquivalentTo(data);
        }
    }
}
