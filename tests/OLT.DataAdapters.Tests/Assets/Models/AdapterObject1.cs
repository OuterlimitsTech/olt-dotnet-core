using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.Assets.Models
{
    public class AdapterObject1
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static AdapterObject1 FakerData()
        {
            var result = new AdapterObject1
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            return result;
        }

        public static List<AdapterObject1> FakerList(int number)
        {
            var list = new List<AdapterObject1>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
