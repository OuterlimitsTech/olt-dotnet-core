using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OLT.Email.SendGrid.Common;
using OLT.Email.SendGrid.Tests.Assets;
using OLT.Libraries.UnitTest.Assets.Email.SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{

    public class SendGridSendTests
    {
        private readonly SendGridProductionConfiguration _prodConfig;

        public SendGridSendTests(IOptions<SendGridProductionConfiguration> options)
        {
            _prodConfig = options.Value;
        }

        [Fact]
        public async Task SendJsonEmail()
        {            
            Assert.NotNull(_prodConfig.ApiKey);

            var firstName = Faker.Name.First();
            var fullName = $"{firstName} Unit Test";

            var template = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);
            template.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
            template.Recipients.To.Add(new OltEmailAddress(Faker.Internet.Email(), $"Unit Test {Faker.Name.Last()}"));

            template.TemplateData.Build.Version = _prodConfig.RunNumber;
            template.TemplateData.Recipient.First = firstName;
            template.TemplateData.Recipient.FullName = fullName;

            //SendGrid uses Newtsoft to Convert, but doesn't give a way to change the resolver, so you have to do it globally. YUCK!!!
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result2 = await OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template)
                .WithCustomArg("EmailUID", Guid.NewGuid().ToString())
                .SendAsync();
            Assert.True(result2.Success);
        }

        [Fact]
        public async Task SendTagEmail()
        {
            Assert.NotNull(_prodConfig.ApiKey);

            var firstName = Faker.Name.First();
            var fullName = $"{firstName} Unit Test";

            var template = TagEmailTemplate.FakerData(_prodConfig.TemplateIdTag);
            template.Recipients.To.Add(new OltEmailAddress(_prodConfig.ToEmail, fullName));
            template.Recipients.To.Add(new OltEmailAddress(Faker.Internet.Email(), $"Unit Test {Faker.Name.Last()}"));

            template.Build.Version = _prodConfig.RunNumber;
            template.Recipient.First = firstName;
            template.Recipient.FullName = fullName;

            ////SendGrid uses Newtsoft to Convert, but doesn't give a way to change the resolver, so you have to do it globally. YUCK!!!
            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};

            var args = OltEmailSendGridExtensions.BuildOltEmailClient(_prodConfig, template).WithCustomArg("EmailUID", Guid.NewGuid().ToString());

            if (_prodConfig.UnsubscribeGroupId > 0)
            {
                args = args.WithUnsubscribeGroupId(_prodConfig.UnsubscribeGroupId.Value);
            }

            var result2 = await args.SendAsync();

            Assert.True(result2.Success);
        }


        [Fact]
        public void ApplicationErrorEmailTests()
        {
            Assert.NotNull(_prodConfig.ApiKey);

            var exceptionMessage = Faker.Lorem.Paragraph(10);
            var appName = $"SendGrid Unit Test of {nameof(ApplicationErrorEmailTests)}";
            var environment = Faker.Company.Name();
            var email = Faker.Internet.Email();
            var ex = new Exception(exceptionMessage);


            var smtpEmail = new OltApplicationErrorEmail
            {
                Subject = Faker.Lorem.Words(34).Last(),
                Body = Faker.Lorem.Paragraph(4),
                AppName = appName,
                Environment = environment,
                From = new OltEmailAddress
                {
                    Name = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                },
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                    {
                        new OltEmailAddress(email)
                    }
                }
            };

            Assert.Throws<Exception>(() => OltEmailSendGridSmtpExtensions.OltEmailError(ex, _prodConfig.ApiKey, smtpEmail, true)); //SENDS EMAIL
            try
            {
                OltEmailSendGridSmtpExtensions.OltEmailError(ex, _prodConfig.ApiKey, smtpEmail, false); //SENDS EMAIL
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }



        [Fact]
        public async Task ExtensionsExceptionsTest()
        {
            var config = new OltEmailConfigurationSendGrid();
            var template = JsonEmailTemplate.FakerData(_prodConfig.TemplateIdJson);
            Assert.Throws<ArgumentNullException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(config, template).Send());  //SHOULD FAIL
            await Assert.ThrowsAsync<ArgumentNullException>(() => OltEmailSendGridExtensions.BuildOltEmailClient(config, template).SendAsync());  //SHOULD FAIL
        }

    }
}
