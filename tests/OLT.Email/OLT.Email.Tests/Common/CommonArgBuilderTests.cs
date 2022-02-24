using FluentAssertions;
using OLT.Email.Tests.Common.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Email.Tests.Common
{
    public class CommonArgBuilderTests
    {

        [Fact]
        public void Recipients()
        {
            var args = new TestArgs();
            var toList = new List<IOltEmailAddress>
            {
                new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()
                }
            };

            var carbonList = new List<IOltEmailAddress>
            {
                new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()
                }
            };

            args = args.WithRecipients(new OltEmailRecipients { To = null, CarbonCopy = null });
            Assert.NotNull(args.ToValue);
            Assert.NotNull(args.CarbonCopyValue);
            Assert.Empty(args.ToValue);
            Assert.Empty(args.CarbonCopyValue);

            var emailRecipients = new OltEmailRecipients
            {
                To = toList,
                CarbonCopy = carbonList
            };

            args = args.WithRecipients(emailRecipients);
            args.ToValue.Should().BeEquivalentTo(toList.OfType<OltEmailAddress>());
            args.CarbonCopyValue.Should().BeEquivalentTo(carbonList.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.To.Should().BeEquivalentTo(toList.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.CarbonCopy.Should().BeEquivalentTo(carbonList.OfType<OltEmailAddress>());

            var duplicate = new List<IOltEmailAddress>();
            duplicate.AddRange(toList);
            duplicate.AddRange(carbonList);

            args = args.WithRecipients(new OltEmailRecipients { To = duplicate, CarbonCopy = duplicate });

            var compareTo = new List<IOltEmailAddress>();
            compareTo.AddRange(toList);
            compareTo.AddRange(duplicate);
            args.ToValue.Should().BeEquivalentTo(compareTo.OfType<OltEmailAddress>());

            compareTo.Clear();
            compareTo.AddRange(carbonList);
            compareTo.AddRange(duplicate);

            args.CarbonCopyValue.Should().BeEquivalentTo(compareTo.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.To.Should().BeEquivalentTo(duplicate.OfType<OltEmailAddress>());
            args.BuildRecipientsValue.CarbonCopy.Should().BeEmpty();

        }

        [Fact]
        public void FromEmail()
        {
            var args = new TestArgs();

            var email = Faker.Internet.Email();

            args = args.WithFromEmail(email);
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Null(args.EmailValue.Name);


            email = Faker.Internet.FreeEmail();
            args.WithFromEmail(new OltEmailAddress(email));
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Null(args.EmailValue.Name);


            var personName = Faker.Name.FullName();

            args = args.WithFromEmail(email, personName);
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Equal(personName, args.EmailValue.Name);

            args.WithFromEmail(new OltEmailAddress(email, personName));
            Assert.Equal(email, args.EmailValue.Email);
            Assert.Equal(personName, args.EmailValue.Name);
            args.ValidationErrors().Should().HaveCount(1);

            args.WithFromEmail(new OltEmailAddress(" ", personName));
            args.ValidationErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress("", personName));
            args.ValidationErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress(null, personName));
            args.ValidationErrors().Should().HaveCount(2);
        }

        [Fact]
        public void Whitelist()
        {
            var whiteEmail = Faker.Internet.Email();
            var whiteDomain = Faker.Internet.DomainName();
            var whiteDomainEmail = $"{Faker.Internet.UserName()}@{whiteDomain}";
            var badDomainEmail = $"{Faker.Internet.UserName()}@{Faker.Internet.DomainName()}";


            var emailList = new List<string>
            {
                whiteEmail,
                Faker.Internet.Email(),
                Faker.Internet.FreeEmail(),
                Faker.Internet.Email(),
                Faker.Internet.FreeEmail(),
            };

            var domainList = new List<string>
            {
                whiteDomain,
                Faker.Internet.DomainName(),
                Faker.Internet.DomainName(),
                Faker.Internet.DomainName()
            };

            var newArgs = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = null });
            Assert.False(newArgs.AllowSend(Faker.Internet.Email()));
            Assert.False(newArgs.AllowSend(badDomainEmail));
            Assert.False(newArgs.AllowSend(whiteEmail));
            Assert.False(newArgs.AllowSend(whiteDomainEmail));


            newArgs = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = null, Email = emailList });
            Assert.False(newArgs.AllowSend(Faker.Internet.Email()));
            Assert.False(newArgs.AllowSend(badDomainEmail));
            Assert.True(newArgs.AllowSend(whiteEmail));
            Assert.False(newArgs.AllowSend(whiteDomainEmail));

            newArgs = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = domainList, Email = null });
            Assert.False(newArgs.AllowSend(Faker.Internet.Email()));
            Assert.False(newArgs.AllowSend(badDomainEmail));
            Assert.False(newArgs.AllowSend(whiteEmail));
            Assert.True(newArgs.AllowSend(whiteDomainEmail));

            newArgs = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailConfigurationWhitelist { Domain = domainList, Email = emailList });
            Assert.False(newArgs.AllowSend(Faker.Internet.Email()));
            Assert.False(newArgs.AllowSend(badDomainEmail));
            Assert.True(newArgs.AllowSend(whiteEmail));
            Assert.True(newArgs.AllowSend(whiteDomainEmail));

            new OltEmailClientSmtp().EnableProductionEnvironment(false).Invoking(args => args.WithWhitelist(new OltEmailAddress { Email = null })).Should().Throw<InvalidOperationException>();
            
            newArgs = new OltEmailClientSmtp().EnableProductionEnvironment(false).WithWhitelist(new OltEmailAddress {  Email = whiteEmail });
            Assert.False(newArgs.AllowSend(Faker.Internet.Email()));
            Assert.False(newArgs.AllowSend(badDomainEmail));
            Assert.True(newArgs.AllowSend(whiteEmail));
            Assert.False(newArgs.AllowSend(whiteDomainEmail));
        }

        [Fact]
        public void Errors()
        {

            var args = new TestArgs();


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

            var errors = args.ValidationErrors();
            Assert.NotEmpty(errors);
            errors.Should().BeEquivalentTo(OltErrorMessages.Recipients, OltErrorMessages.From);
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithFromEmail(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithFromEmail(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithRecipients(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithAttachment(null)).Should().Throw<ArgumentNullException>();

            OltEmailConfigurationWhitelist config = null;
            OltEmailAddress emailAddress = null;
            args.Invoking(args => args.WithWhitelist(config)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithWhitelist(emailAddress)).Should().Throw<ArgumentNullException>();

            var msg = Faker.Lorem.GetFirstWord();
            var testEx = new OltEmailValidationException(compareErrors, msg);
            testEx.Errors.Should().BeEquivalentTo(compareErrors);
            Assert.Equal(msg, testEx.Message);

        }
    }
}