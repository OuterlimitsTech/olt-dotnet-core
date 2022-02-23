using FluentAssertions;
using OLT.Email.SendGrid.Tests.Assets;
using OLT.Email.SendGrid.Tests.Assets.SendGrid.Json;
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
            Assert.Null(args.TemplateValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltArgErrorsCommon.Recipients, OltArgErrorsCommon.From, OltArgErrorsSendGrid.TemplateId);
        }

        [Fact]
        public void WithTemplate()
        {
            var args = new SendGridTemplateTestArgs();
            var template = JsonEmailTemplate.FakerData();

            args = args.WithTemplate(template);
            args.TemplateValue.Should().BeEquivalentTo(template);
            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltArgErrorsCommon.Recipients, OltArgErrorsCommon.From, OltArgErrorsSendGrid.ApiKey);
        }

        [Fact]
        public void WithUnsubscribeGroupId()
        {
            var args = new SendGridTemplateTestArgs();

            var value = Faker.RandomNumber.Next(10, 100);

            args = args.WithUnsubscribeGroupId(value);
            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.TemplateValue);
            Assert.Equal(value, args.UnsubscribeGroupIdValue);
            Assert.True(args.ClickTrackingValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltArgErrorsCommon.Recipients, OltArgErrorsCommon.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
        }

        [Fact]
        public void WithoutClickTracking()
        {
            var args = new SendGridTemplateTestArgs();

            args = args.WithoutClickTracking();
            Assert.Null(args.ApiKeyValue);
            Assert.Null(args.TemplateValue);
            Assert.False(args.ClickTrackingValue);
            Assert.Null(args.UnsubscribeGroupIdValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltArgErrorsCommon.Recipients, OltArgErrorsCommon.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
        }

        [Fact]
        public void Errors()
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

            var errors = args.GetErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltArgErrorsCommon.Recipients, OltArgErrorsCommon.From, OltArgErrorsSendGrid.TemplateId, OltArgErrorsSendGrid.ApiKey);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithApiKey(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithTemplate(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithUnsubscribeGroupId(0)).Should().Throw<ArgumentOutOfRangeException>();
            args.Invoking(args => args.WithUnsubscribeGroupId(-1)).Should().Throw<ArgumentOutOfRangeException>();

        }
    }
}

