using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using System;

namespace OLT.Core
{
    public class OltAwsBasicProfile
    {
        public OltAwsBasicProfile(RegionEndpoint region, string accessKey, string secretKey, string profileName = "default")
        {

            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new ArgumentNullException(nameof(accessKey));
            }

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }


            if (string.IsNullOrWhiteSpace(profileName))
            {
                profileName = "default";
            }

            ProfileName = profileName;
            Region = region;
            AccessKey = accessKey;
            SecretKey = secretKey;
        }

        public string ProfileName { get; }
        public RegionEndpoint Region { get; }
        public string AccessKey { get; }
        public string SecretKey { get; }

        public AWSOptions Build()
        {
            return new AWSOptions
            {
                Profile = ProfileName,
                Region = Region,
                Credentials = new BasicAWSCredentials(AccessKey, SecretKey),
            };
        }
    }
}