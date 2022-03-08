using FluentAssertions;
using OLT.Constants;
using OLT.Email.SendGrid.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.SendGrid.Tests
{
    public class SendGridArgBuilderTests
    {
        [Fact]
        public void WithApiKey()
        {
            var args = new SendGridTemplateTestArgs();

            var value = Faker.Lorem.GetFirstWord();

            args = args.WithApiKey(value);
            Assert.Equal(value, args.ApiKeyValue);
            Assert.Empty(args.CustomArgsValue);
            Assert.Null(args.TemplateValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.TemplateId);
        }

        [Fact]
        public void WithTemplate()
        {
            var args = new SendGridTemplateTestArgs();
            var template = FakeJsonEmailTemplate.FakerData(2, 2);

            args = args.WithTemplate(template);
            args.TemplateValue.Should().BeEquivalentTo(template);
            Assert.Empty(args.CustomArgsValue);
            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.False(args.SandboxModeValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.ApiKey);
        }

        [Fact]
        public void WithCustomArg()
        {
            var args = new SendGridTemplateTestArgs();

            var dict = new Dictionary<string, string>();
            dict.Add(Faker.Internet.DomainName(), Faker.Lorem.GetFirstWord());
            dict.Add(Faker.Internet.DomainName(), Faker.Lorem.Paragraph(1));

            foreach(var kvp in dict)
            {
                args = args.WithCustomArg(kvp.Key, kvp.Value);
            }

            args.CustomArgsValue.Should().BeEquivalentTo(dict);
            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);
            Assert.False(args.SandboxModeValue);
            Assert.Null(args.TemplateValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.ApiKey, OltArgErrorsSendGrid.TemplateId);
        }

        [Fact]
        public void EnableSandbox()
        {
            var args = new SendGridTemplateTestArgs();

            args = args.EnableSandbox();

            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);
            Assert.True(args.SandboxModeValue);
            Assert.Null(args.TemplateValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.ApiKey, OltArgErrorsSendGrid.TemplateId);
        }


        [Fact]
        public void WithUnsubscribeGroupId()
        {
            var args = new SendGridTemplateTestArgs();

            var value = Faker.RandomNumber.Next(10, 100);

            args = args.WithUnsubscribeGroupId(value);
            Assert.Null(args.ApiKeyValue);
            Assert.Empty(args.CustomArgsValue);
            Assert.Null(args.TemplateValue);
            Assert.Equal(value, args.UnsubscribeGroupIdValue);
            Assert.False(args.SandboxModeValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
        }

        [Fact]
        public void WithoutClickTracking()
        {
            var args = new SendGridTemplateTestArgs();

            args = args.WithoutClickTracking();
            Assert.Null(args.ApiKeyValue);
            Assert.Empty(args.CustomArgsValue);
            Assert.Null(args.TemplateValue);
            Assert.False(args.ClickTrackingValue);
            Assert.False(args.SandboxModeValue);
            Assert.Null(args.UnsubscribeGroupIdValue);

            var errors = args.ValidationErrors();
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
                        
        }

        [Fact]
        public void TemplateErrors()
        {

            var args = new SendGridTemplateTestArgs();

            args.Invoking(args => args.DoValidation()).Should().Throw<OltSendGridValidationException>().WithMessage(OltSendGridValidationException.DefaultMessage);

            List<string> compareErrors = new List<string>();
            try
            {
                args.DoValidation();
            }
            catch (OltSendGridValidationException ex)
            {
                compareErrors = ex.Errors;
                var errorResult = ex.ToEmailResult();
                compareErrors.Should().Equal(errorResult.Errors);
                Assert.False(errorResult.Success);
            }

            var errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithApiKey(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithTemplate(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithUnsubscribeGroupId(0)).Should().Throw<ArgumentOutOfRangeException>();
            args.Invoking(args => args.WithUnsubscribeGroupId(-1)).Should().Throw<ArgumentOutOfRangeException>();
            args.Invoking(args => args.WithCustomArg(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithCustomArg(Faker.Company.Name(), null)).Should().Throw<ArgumentNullException>();

            var smtpArgs = new OltEmailClientSmtpSendGrid();

            smtpArgs.Invoking(args => args.WithApiKey(null)).Should().Throw<ArgumentNullException>();

            try
            {
                args.DoValidation();
            }
            catch (OltSendGridValidationException ex)
            {
                compareErrors = ex.Errors;
                var errorResult = ex.ToEmailResult();
                compareErrors.Should().Equal(errorResult.Errors);
                Assert.False(errorResult.Success);
            }


        }


        [Fact]
        public void SmtpClientErrors()
        {
            var args = new OltEmailClientSmtpSendGrid();
            var errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body, OltArgErrorsSendGrid.ApiKey);            
            args.Invoking(args => args.WithApiKey(null)).Should().Throw<ArgumentNullException>();

            args.WithApiKey(Faker.Internet.UserName());
            errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);

        }

        [Fact]
        public void SmtpClientWithApiKey()
        {
            var args = new OltEmailClientSmtpSendGrid();
            args = args.WithApiKey(Faker.Internet.UserName());

            var errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltEmailErrors.Recipients, OltEmailErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);

        }
    }
}

