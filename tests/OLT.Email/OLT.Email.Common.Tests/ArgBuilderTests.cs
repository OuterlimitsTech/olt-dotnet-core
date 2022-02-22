using FluentAssertions;
using OLT.Email.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Email.Common.Tests
{

    public class ArgBuilderTests
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

            args = args.WithRecipients(new OltEmailRecipients {  To = null, CarbonCopy = null });
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
            
            args = args.WithRecipients(new OltEmailRecipients {  To = duplicate, CarbonCopy = duplicate });

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
            args.GetErrors().Should().HaveCount(1);

            args.WithFromEmail(new OltEmailAddress(" ", personName));
            args.GetErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress("", personName));
            args.GetErrors().Should().HaveCount(2);

            args.WithFromEmail(new OltEmailAddress(null, personName));
            args.GetErrors().Should().HaveCount(2);
        }

        [Fact]
        public void Errors()
        {

            var args = new TestArgs();


            args.Invoking(args => args.DoValidation()).Should().Throw<TestException>().WithMessage("Test Validation");

            List<string> compareErrors = new List<string>();
            try
            {
                args.DoValidation();
            }
            catch (TestException ex)
            {
                compareErrors = ex.Errors;
                var errorResult = ex.ToEmailResult();
                compareErrors.Should().Equal(errorResult.Errors);
                Assert.False(errorResult.Success);
            }

            var errors = args.GetErrors();
            Assert.NotEmpty(errors);
            errors.Should().HaveCount(2);
            errors.Should().BeEquivalentTo("Requires To Recipient", "From Email Missing");
            errors.Should().BeEquivalentTo(compareErrors);

            args.Invoking(args => args.WithFromEmail(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithFromEmail(null, null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithRecipients(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithAttachment(null)).Should().Throw<ArgumentNullException>();
            args.Invoking(args => args.WithWhitelist(null)).Should().Throw<ArgumentNullException>();

        }
    }
}