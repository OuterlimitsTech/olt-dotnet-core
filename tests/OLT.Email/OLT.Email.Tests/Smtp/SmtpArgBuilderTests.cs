using FluentAssertions;
using OLT.Email.Tests.Smtp.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Email.Tests.Smtp
{
    public class SmtpArgBuilderTests
    {
        [Fact]
        public void WithSmtpHost()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.GetFirstWord();

            args = args.WithSmtpHost(value);
            Assert.Equal(value, args.SmtpHostValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        public void WithSmtpSSLDisabled()
        {
            var args = new SmtpTestArgs();

            Assert.False(args.SmtpSSLDisabledValue);
            args = args.WithSmtpSSLDisabled(true);
            Assert.True(args.SmtpSSLDisabledValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        public void WithSmtpPort()
        {
            var args = new SmtpTestArgs();

            var value = Convert.ToInt16(Faker.RandomNumber.Next(1, short.MaxValue));

            args = args.WithSmtpPort(value);
            Assert.Equal(value, args.SmtpPortValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }

        [Fact]
        public void WithSubject()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.GetFirstWord();

            args = args.WithSubject(value);
            Assert.Equal(value, args.SubjectLineValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Body);
        }


        [Fact]
        public void WithAppError()
        {
            var args = new SmtpTestArgs();
            var errorMsg = Faker.Lorem.Paragraph(10);
            var appName = Faker.Lorem.Words(20).Last();
            var environment = Faker.Lorem.Words(10).Last();

            var ex = new Exception(errorMsg);
            

            args = args.WithAppError(ex, appName, environment);
            Assert.StartsWith($"[{appName}] APPLICATION ERROR in {environment} Environment occurred at", args.SubjectLineValue);
            Assert.Equal($"The following error occurred:{Environment.NewLine}{ex}", args.BodyValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host);
        }

        [Fact]
        public void WithBody()
        {
            var args = new SmtpTestArgs();

            var value = Faker.Lorem.Paragraph(10);

            args = args.WithBody(value);
            Assert.Equal(value, args.BodyValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject);
        }

        [Fact]
        public void WithSmtpNetworkCredentials()
        {
            var args = new SmtpTestArgs();

            var username = Faker.Internet.UserName();
            var password = Faker.Internet.DomainWord();

            args = args.WithSmtpNetworkCredentials(username, password);
            Assert.Equal(username, args.SmtpUsernameValue);
            Assert.Equal(password, args.SmtpPasswordValue);

            var errors = args.GetErrors();
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
        }


        [Fact]
        public void Errors()
        {

            var args = new SmtpTestArgs();

            args.Invoking(args => args.DoValidation()).Should().Throw<OltEmailValidationException>().WithMessage(OltEmailValidationException.DefaultMessage);

            List<string> compareErrors = new List<string>();
            try
            {
                args.DoValidation();
            }
            catch (OltEmailValidationException ex)
            {
                compareErrors = ex.Errors;
                var errorResult = ex.ToEmailResult();
                compareErrors.Should().Equal(errorResult.Errors);
                Assert.False(errorResult.Success);
            }

            var errors = args.GetErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltCommonArgErrors.Recipients, OltCommonArgErrors.From, OltSmtpArgErrors.Host, OltSmtpArgErrors.Subject, OltSmtpArgErrors.Body);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithSmtpNetworkCredentials(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpNetworkCredentials(Faker.Internet.UserName(), null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpNetworkCredentials(null, Faker.Lorem.GetFirstWord())).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithBody(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSubject(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithSmtpPort(0)).Should().Throw<ArgumentOutOfRangeException>();
            args.Invoking(args => args.WithSmtpHost(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithCalendarInvite(null)).Should().Throw<ArgumentNullException>();

        }
    }
}
