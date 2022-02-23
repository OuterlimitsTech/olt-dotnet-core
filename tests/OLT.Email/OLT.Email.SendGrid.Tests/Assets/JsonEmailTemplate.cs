using System.Collections.Generic;
using System.Linq;
using OLT.Core;
using OLT.Email;

namespace OLT.Email.SendGrid.Tests.Assets.SendGrid.Json
{
    public class EmailDataCommunicationJson
    {
        public string Body1 { get; set; }
        public string Body2 { get; set; }
    }

    public class EmailDataBuildVersionJson
    {
        public string Version { get; set; }
    }

    public class EmailDataJson
    {
        public EmailDataCommunicationJson Communication { get; set; } = new EmailDataCommunicationJson();
        public EmailDataBuildVersionJson Build { get; set; } = new EmailDataBuildVersionJson();

        public static EmailDataJson FakerData()
        {
            return new EmailDataJson
            {
                Build = new EmailDataBuildVersionJson
                {
                    Version = Faker.Country.Name()
                },
                Communication = new EmailDataCommunicationJson
                {
                    Body1 = Faker.Lorem.Sentences(4).LastOrDefault(),
                    Body2 = Faker.Lorem.Sentences(10).LastOrDefault(),
                }
            };
        }
    }

    public class JsonEmailTemplate : OltEmailJsonTemplate<OltEmailAddress, EmailDataJson>, IOltEmailTemplate<OltEmailAddress>
    {
        public override string TemplateId => nameof(JsonEmailTemplate);
        public override List<OltEmailAddress> To { get; set; } = new List<OltEmailAddress>();
        public override EmailDataJson TemplateData { get; set; }

        public static JsonEmailTemplate FakerData()
        {
            return new JsonEmailTemplate
            {
                To = new List<OltEmailAddress>
                {
                    new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                    new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                },
                TemplateData = EmailDataJson.FakerData(),
            };
        }

    }
}