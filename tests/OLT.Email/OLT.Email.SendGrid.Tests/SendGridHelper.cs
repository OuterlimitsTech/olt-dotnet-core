using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Email.SendGrid.Tests
{
    public static class SendGridHelper
    {
        public static IOltEmailAddress FakerEmailAddress()
        {
            return new OltEmailAddress
            {
                Name = Faker.Name.FullName(),
                Email = Faker.Internet.FreeEmail()
            };
        }

        public static OltEmailConfigurationWhitelist BuildWhitelist(List<IOltEmailAddress> emailAddresses)
        {
            var config = new OltEmailConfigurationWhitelist();
            config.Email.AddRange(emailAddresses.Select(p => p.Email));
            return config;
        }


        public static OltEmailConfigurationSendGrid FakerConfig(bool production, int numEmailWhitelist, int numDomainWhitelist)
        {
            var result = new OltEmailConfigurationSendGrid
            {
                ApiKey = Faker.Company.Name(),
                Production = production,
                From = new OltEmailAddress
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName(),
                },
            };

            for(int i = 0; i < numEmailWhitelist; i++)
            {
                result.TestWhitelist.Email.Add(FakerEmailAddress().Email);
            }

            for (int i = 0; i < numDomainWhitelist; i++)
            {
                result.TestWhitelist.Email.Add(Faker.Internet.DomainName());
            }

            return result;
        }
    }
}
