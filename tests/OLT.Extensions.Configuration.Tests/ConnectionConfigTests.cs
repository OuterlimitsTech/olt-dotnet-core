using Xunit;
using OLT.Core;

namespace OLT.Extensions.Configuration.Tests
{
    public class ConnectionConfigTests
    {

        [Theory]        
        [InlineData("host=rabbitmq://localhost:5672;username=guest;password=abc123", "rabbitmq://localhost:5672", "guest", "abc123")]
        [InlineData("host=rabbitmq://localhost:5672;username=guest;password=", "rabbitmq://localhost:5672", "guest", null)]
        [InlineData("host=rabbitmq://localhost:5672;username=guest", "rabbitmq://localhost:5672", "guest", null)]
        [InlineData("host=rabbitmq://localhost:5672;password=abc123", "rabbitmq://localhost:5672", null, "abc123")]
        [InlineData("host=rabbitmq://localhost:5672;username=;password=abc123", "rabbitmq://localhost:5672", null, "abc123")]
        [InlineData("username=guest;password=abc123", null, "guest", "abc123")]
        [InlineData("host=;username=guest;password=abc123", null, "guest", "abc123")]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
        [InlineData("", null, null, null)]
        [InlineData(" ", null, null, null)]
        [InlineData(null, null, null, null)]
        public void RabbitMq(string connectionString, string expectedHost, string expectedUsername, string expectedPassword)
        {
            var config = new OltConnectionConfigRabbitMq();
            config.Parse(connectionString);

            Assert.Equal(expectedHost, config.Host);
            Assert.Equal(expectedUsername, config.Username);
            Assert.Equal(expectedPassword, config.Password);
        }


        [Theory]
        [InlineData("region=us-east-2;accessKey=987xyz;secretKey=abc123", "us-east-2", "987xyz", "abc123")]
        [InlineData("region=us-east-2;accessKey=987xyz;secretKey=", "us-east-2", "987xyz", null)]
        [InlineData("region=us-east-2;accessKey=987xyz", "us-east-2", "987xyz", null)]
        [InlineData("region=us-east-2;secretKey=abc123", "us-east-2", null, "abc123")]
        [InlineData("region=us-east-2;accessKey=;secretKey=abc123", "us-east-2", null, "abc123")]
        [InlineData("region=;accessKey=987xyz;secretKey=abc123", null, "987xyz", "abc123")]
        [InlineData("accessKey=987xyz;secretKey=abc123", null, "987xyz", "abc123")]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
        [InlineData("", null, null, null)]
        [InlineData(" ", null, null, null)]
        [InlineData(null, null, null, null)]
        public void Amazon(string connectionString, string expectedRegion, string expectedUsername, string expectedPassword)
        {
            var config = new OltConnectionConfigAmazon();
            config.Parse(connectionString);

            Assert.Equal(expectedRegion, config.Region);
            Assert.Equal(expectedUsername, config.AccessKey);
            Assert.Equal(expectedPassword, config.SecretKey);
        }


        [Theory]
        [InlineData("endpoint=https://api.domain.com;apikey=abc123", "https://api.domain.com/", "abc123")]
        [InlineData("endpoint=https://api.domain.com/;apikey=abc123", "https://api.domain.com/", "abc123")]
        [InlineData("endpoint=https://api.domain.com;apikey=", "https://api.domain.com/", null)]
        [InlineData("endpoint=https://api.domain.com", "https://api.domain.com/", null)]
        [InlineData("endpoint=https://api.domain.com/", "https://api.domain.com/", null)]
        [InlineData("endpoint=;apikey=abc123", null, "abc123")]
        [InlineData("apikey=abc123", null, "abc123")]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null)]
        [InlineData("", null, null)]
        [InlineData(" ", null, null)]
        [InlineData(null, null, null)]
        public void ApiKey(string connectionString, string expectedEndpoint, string expectedApiKey)
        {
            var config = new OltConnectionConfigApiKey();
            config.Parse(connectionString);

            Assert.Equal(expectedEndpoint, config.Endpoint);
            Assert.Equal(expectedApiKey, config.ApiKey);
        }


        [Theory]
        [InlineData("endpoint=https://api.domain.com/service.svc;username=guest;password=abc123", "https://api.domain.com/service.svc", "guest", "abc123")]
        [InlineData("endpoint=https://api.domain.com/service.svc;username=guest;password=", "https://api.domain.com/service.svc", "guest", null)]
        [InlineData("endpoint=https://api.domain.com/service.svc;username=guest", "https://api.domain.com/service.svc", "guest", null)]
        [InlineData("endpoint=https://api.domain.com/service.svc;password=abc123", "https://api.domain.com/service.svc", null, "abc123")]
        [InlineData("endpoint=https://api.domain.com/service.svc;username=;password=abc123", "https://api.domain.com/service.svc", null, "abc123")]
        [InlineData("username=guest;password=abc123", null, "guest", "abc123")]
        [InlineData("host=;username=guest;password=abc123", null, "guest", "abc123")]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null)]
        [InlineData("", null, null, null)]
        [InlineData(" ", null, null, null)]
        [InlineData(null, null, null, null)]
        public void Wcf(string connectionString, string expectedEndpoint, string expectedUsername, string expectedPassword)
        {
            var config = new OltConnectionConfigWcf();
            config.Parse(connectionString);

            Assert.Equal(expectedEndpoint, config.Endpoint);
            Assert.Equal(expectedUsername, config.Username);
            Assert.Equal(expectedPassword, config.Password);
        }


        [Theory]
        [InlineData("host=localhost;port=41;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 41, "/root/test")]
        [InlineData("host=localhost;port=42;username=guest;password=abc123;workingdir=", "localhost", "guest", "abc123", 42, null)]
        [InlineData("host=localhost;port=43;username=guest;password=abc123", "localhost", "guest", "abc123", 43, null)]
        [InlineData("host=localhost;port=44;username=guest;password=;workingdir=/root/test", "localhost", "guest", null, 44, "/root/test")]
        [InlineData("host=localhost;port=45;username=guest;workingdir=/root/test", "localhost", "guest", null, 45, "/root/test")]
        [InlineData("host=localhost;port=46;username=;password=abc123;workingdir=/root/test", "localhost", null, "abc123", 46, "/root/test")]
        [InlineData("host=localhost;port=47;password=abc123;workingdir=/root/test", "localhost", null, "abc123", 47, "/root/test")]
        [InlineData("host=localhost;port=;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 22, "/root/test")]
        [InlineData("host=localhost;username=guest;password=abc123;workingdir=/root/test", "localhost", "guest", "abc123", 22, "/root/test")]
        [InlineData("host=;port=48;username=guest;password=abc123;workingdir=/root/test", null, "guest", "abc123", 48, "/root/test")]
        [InlineData("port=49;username=guest;password=abc123;workingdir=/root/test", null, "guest", "abc123", 49, "/root/test")]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null, 22, null)]        
        [InlineData("", null, null, null, 22, null)]
        [InlineData(" ", null, null, null, 22, null)]
        [InlineData(null, null, null, null, 22, null)]
        public void Sftp(string connectionString, string expectedHost, string expectedUsername, string expectedPassword, int? expectedPort, string expectedWorkingDir)
        {
            var config = new OltConnectionConfigSftp();
            config.Parse(connectionString);

            Assert.Equal(expectedHost, config.Host);
            Assert.Equal(expectedUsername, config.Username);
            Assert.Equal(expectedPassword, config.Password);
            Assert.Equal(expectedPort, config.Port);
            Assert.Equal(expectedWorkingDir, config.WorkingDirectory);
        }


        [Theory]
        [InlineData("host=localhost;port=41;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 41, true)]
        [InlineData("host=localhost;port=41;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 41, false)]
        [InlineData("host=localhost;port=42;username=guest;password=abc123;workingdir=", "localhost", "guest", "abc123", 42, null)]
        [InlineData("host=localhost;port=43;username=guest;password=abc123", "localhost", "guest", "abc123", 43, null)]
        [InlineData("host=localhost;port=44;username=guest;password=;ssl=true", "localhost", "guest", null, 44, true)]
        [InlineData("host=localhost;port=44;username=guest;password=;ssl=false", "localhost", "guest", null, 44, false)]
        [InlineData("host=localhost;port=45;username=guest;ssl=true", "localhost", "guest", null, 45, true)]
        [InlineData("host=localhost;port=45;username=guest;ssl=false", "localhost", "guest", null, 45, false)]
        [InlineData("host=localhost;port=46;username=;password=abc123;ssl=true", "localhost", null, "abc123", 46, true)]
        [InlineData("host=localhost;port=46;username=;password=abc123;ssl=false", "localhost", null, "abc123", 46, false)]
        [InlineData("host=localhost;port=47;password=abc123;ssl=true", "localhost", null, "abc123", 47, true)]
        [InlineData("host=localhost;port=47;password=abc123;ssl=false", "localhost", null, "abc123", 47, false)]
        [InlineData("host=localhost;port=;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 587, true)]
        [InlineData("host=localhost;port=;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 587, false)]
        [InlineData("host=localhost;username=guest;password=abc123;ssl=true", "localhost", "guest", "abc123", 587, true)]
        [InlineData("host=localhost;username=guest;password=abc123;ssl=false", "localhost", "guest", "abc123", 587, false)]
        [InlineData("host=;port=48;username=guest;password=abc123;ssl=true", null, "guest", "abc123", 48, true)]
        [InlineData("host=;port=48;username=guest;password=abc123;ssl=false", null, "guest", "abc123", 48, false)]
        [InlineData("port=49;username=guest;password=abc123;ssl=true", null, "guest", "abc123", 49, true)]
        [InlineData("server=serverValue;another=anotherValue;test=testValue", null, null, null, 587, null)]
        [InlineData("", null, null, null, 587, null)]
        [InlineData(" ", null, null, null, 587, null)]
        [InlineData(null, null, null, null, 587, null)]
        public void Smtp(string connectionString, string expectedHost, string expectedUsername, string expectedPassword, int? expectedPort, bool? expectedSsl)
        {
            var config = new OltConnectionConfigSmtp();
            config.Parse(connectionString);

            Assert.Equal(expectedHost, config.Host);
            Assert.Equal(expectedUsername, config.Username);
            Assert.Equal(expectedPassword, config.Password);
            Assert.Equal(expectedPort, config.Port);
            Assert.Equal(expectedSsl, config.EnableSsl);
        }
    }
}