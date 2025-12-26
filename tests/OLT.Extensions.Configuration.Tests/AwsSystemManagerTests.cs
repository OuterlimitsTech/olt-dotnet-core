using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using AwesomeAssertions;
using Microsoft.Extensions.Configuration;
using OLT.Core;

namespace OLT.Extensions.Configuration.Tests
{
    public class AwsSystemManagerTests
    {
        private static string RunEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";


        public static TheoryData<OltAwsBasicProfile, AWSOptions> BaseProfileData
        {
            get
            {                
                var secretKey = Faker.Lorem.Words(7).Last();
                var accessKey = Faker.Lorem.Words(11).Last();
                var profileName = Faker.Lorem.Words(20).Last();
                var region = RegionEndpoint.USWest2;

                var expected = new AWSOptions
                {
                    Profile = profileName,
                    Region = region,
                    Credentials = new BasicAWSCredentials(accessKey, secretKey)
                };

                var results = new TheoryData<OltAwsBasicProfile, AWSOptions>();
                results.Add(new OltAwsBasicProfile(region, accessKey, secretKey, profileName), expected);
                results.Add(new OltAwsBasicProfile(region, accessKey, secretKey), new AWSOptions {  Profile = "default", Region = region, Credentials = new BasicAWSCredentials(accessKey, secretKey) });
                return results;
            }
        }

        [Theory]
        [MemberData(nameof(BaseProfileData))]
        public void BaseProfileTests(OltAwsBasicProfile value, AWSOptions expected)
        {
            var result = value.Build();

            Assert.Equal(result.Credentials.GetCredentials().AccessKey, expected.Credentials.GetCredentials().AccessKey);
            Assert.Equal(result.Credentials.GetCredentials().SecretKey, expected.Credentials.GetCredentials().SecretKey);
            Assert.Equal(result.Region, expected.Region);
            Assert.Equal(result.Profile, expected.Profile);
        }

        public static TheoryData<OltAwsConfigurationOptions, string, string> ConfigOptionsData
        {
            get
            {
                var profile = new OltAwsBasicProfile(RegionEndpoint.USEast2, Faker.Internet.UserName(), Faker.Lorem.GetFirstWord(), Faker.Lorem.Words(10).Last());
                var rootPath = Faker.Lorem.Words(15).Last();
                var fallbackDefault = Faker.Lorem.Words(23).Last();
                var expectedFallback = $"/{rootPath}/{fallbackDefault}/";
                var expectedEnvironment = $"/{rootPath}/{RunEnvironment}/";

                var results = new TheoryData<OltAwsConfigurationOptions, string, string>();
                results.Add(new OltAwsConfigurationOptions(profile, rootPath, RunEnvironment, fallbackDefault), expectedFallback, expectedEnvironment);
                results.Add(new OltAwsConfigurationOptions(profile, $"/{rootPath}", $"/{RunEnvironment}", $"/{fallbackDefault}"), expectedFallback, expectedEnvironment);
                results.Add(new OltAwsConfigurationOptions(profile, $"/{rootPath}/", $"/{RunEnvironment}/", $"/{fallbackDefault}/"), expectedFallback, expectedEnvironment);
                results.Add(new OltAwsConfigurationOptions(profile, $"{rootPath}/", $"{RunEnvironment}/", $"{fallbackDefault}/"), expectedFallback, expectedEnvironment);

                return results;
            }
        }

        [Theory]
        [MemberData(nameof(ConfigOptionsData))]
        public void PathTests(OltAwsConfigurationOptions value, string expectedFallback, string expectedEnvironment)
        {            
            Assert.Equal(value.BuildPathFallback(), expectedFallback);
            Assert.Equal(value.BuildPathEnvironment(), expectedEnvironment);
        }


        [Fact]
        public void ConnectionTests()
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<AzureAppConfigTests>();

            var configuration = builder.Build();

            try
            {
                var accessKey = configuration.GetValue<string>("AWS_ACCESS_KEY") ?? Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
                var secretKey = configuration.GetValue<string>("AWS_SECRET_KEY") ?? Environment.GetEnvironmentVariable("AWS_SECRET_KEY");

                var profile = new OltAwsBasicProfile(RegionEndpoint.USEast2, accessKey, secretKey);
                builder.AddSystemsManager(new OltAwsConfigurationOptions(profile, "UnitTests", RunEnvironment, "Default"), TimeSpan.FromMinutes(3), false);

                var awsConfig = builder.Build();
                Assert.NotNull(awsConfig);
                var value = awsConfig.GetValue<string>("Value");
                Assert.Equal($"1234ABC-{RunEnvironment ?? "DEFAULT"}", value);
            }
            catch (Exception)
            {
                throw;
            }

          
        }


        [Fact]
        public void ExceptionTest()
        {
            var profile = new OltAwsBasicProfile(RegionEndpoint.USEast2, Faker.Internet.UserName(), Faker.Lorem.GetFirstWord(), Faker.Lorem.Words(10).Last());

            Assert.Throws<ArgumentNullException>("region", () => new OltAwsBasicProfile(null, null, null, null));
            Assert.Throws<ArgumentNullException>("accessKey", () => new OltAwsBasicProfile(profile.Region, null, null, null));
            Assert.Throws<ArgumentNullException>("secretKey", () => new OltAwsBasicProfile(profile.Region, profile.AccessKey, null, null));

            Action act = () => new OltAwsBasicProfile(profile.Region, profile.AccessKey, profile.SecretKey, null);
            act.Should().NotThrow<ArgumentNullException>();


            Assert.Throws<ArgumentNullException>("profile", () => new OltAwsConfigurationOptions(null, null, null, null));
            Assert.Throws<ArgumentNullException>("rootPath", () => new OltAwsConfigurationOptions(profile, null, null, null));
            Assert.Throws<ArgumentNullException>("environmentName", () => new OltAwsConfigurationOptions(profile, Faker.Lorem.Words(11).Last(), null, null));

            act = () => new OltAwsConfigurationOptions(profile, Faker.Lorem.Words(11).Last(), RunEnvironment, null);
            act.Should().NotThrow<ArgumentNullException>();

            Assert.Throws<ArgumentNullException>("builder", () => OltAmazonConfigurationBuilderExtensions.AddSystemsManager(null, null, null, true));
            Assert.Throws<ArgumentNullException>("options", () => OltAmazonConfigurationBuilderExtensions.AddSystemsManager(new ConfigurationBuilder(), null, null, true));


            var opts = new OltAwsConfigurationOptions(profile, Faker.Lorem.Words(11).Last(), RunEnvironment, null);
            act = () => OltAmazonConfigurationBuilderExtensions.AddSystemsManager(new ConfigurationBuilder(), opts, null, false);
            act.Should().NotThrow<ArgumentNullException>();

            act = () => OltAmazonConfigurationBuilderExtensions.AddSystemsManager(new ConfigurationBuilder(), opts, TimeSpan.FromSeconds(20), false);
            act.Should().NotThrow<ArgumentNullException>();

        }
    }
}
