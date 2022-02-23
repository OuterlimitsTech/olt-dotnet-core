using System.Collections.Generic;

namespace OLT.Email.SendGrid.Tests.Assets
{
    public class FakeJsonEmailTemplate : JsonEmailTemplate
    {
        public FakeJsonEmailTemplate() : base(nameof(FakeJsonEmailTemplate))
        {

        }

        public static FakeJsonEmailTemplate FakerData()
        {
            return new FakeJsonEmailTemplate
            {
                Recipients = new OltEmailRecipients
                {
                    To = new List<IOltEmailAddress>
                   {
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                       new OltEmailAddress(Faker.Internet.Email(), Faker.Name.First()),
                   }
                },
                TemplateData = EmailDataJson.FakerData(),
            };
        }

    }
}