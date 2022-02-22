using FluentAssertions;
using OLT.Email.Common.Tests.Assets;
using System.Collections.Generic;
using Xunit;

namespace OLT.Email.Common.Tests
{
    public class TemplateTests
    {
        [Fact]
        public void SingleEmail()
        {
            var template = new SingleEmailTagTemplate();
            var firstName = Faker.Name.First();
            var lastName = Faker.Name.Last();
            var email = Faker.Internet.Email();

            template.FirstName = firstName;
            template.LastName = lastName;
            template.To = new OltEmailAddress(email, firstName);


            Assert.Equal(firstName, template.FirstName);
            Assert.Equal(lastName, template.LastName);
            Assert.Equal(email, template.To.Email);
            Assert.Equal(firstName, template.To.Name);
            Assert.Equal(nameof(SingleEmailTagTemplate), template.TemplateName);
            Assert.NotEmpty(template.Tags);

        }


        [Fact]
        public void MultipleEmail()
        {
            var template = new EmailTagTemplate();
            var firstName1 = Faker.Name.First();
            var email1 = Faker.Internet.Email();

            var firstName2 = Faker.Name.First();
            var email2 = Faker.Internet.Email();


            template.Value1 = Faker.Internet.DomainName();
            template.Value2 = Faker.Internet.DomainSuffix();

            template.To.Add(new OltEmailAddress(email1, firstName1));
            template.To.Add(new OltEmailAddress(email2, firstName2));

            var list = new List<OltEmailAddress>
            {
                new OltEmailAddress(email1, firstName1),
                new OltEmailAddress(email2, firstName2)
            };

            template.To.Should().BeEquivalentTo(list);

            Assert.Equal(nameof(EmailTagTemplate), template.TemplateName);
            Assert.NotEmpty(template.Tags);

        }
    }
}