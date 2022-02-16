using Xunit;

namespace OLT.Email.Smtp.Tests
{
    public class ConfigurationTests
    {
        [Fact]
        public void OltSmtpConfiguration()
        {
            var server = Faker.Internet.DomainName();
            var port = Faker.RandomNumber.Next();
            var username = Faker.Internet.UserName();
            var password = Faker.Lorem.GetFirstWord();

            var model = new OltSmtpConfiguration();
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

        }
    }
}