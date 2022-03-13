using System.Collections.Generic;

namespace OLT.Core.Common.Tests.Assets
{
    public class TestPersonNameModel
    {
        public string NameFirst { get; set; }
        public string NameLast { get; set; }

        public static List<TestPersonNameModel> FakerList(int number)
        {
            var list = new List<TestPersonNameModel>();

            for (int i = 0; i < number; i++)
            {
                list.Add(new TestPersonNameModel
                {
                    NameFirst = Faker.Name.First(),
                    NameLast = Faker.Name.Last(),
                });
            }

            return list;
        }

    }
}
