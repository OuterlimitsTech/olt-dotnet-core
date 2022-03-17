using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.AdapterTests.Models
{
    public class BasicAdapterObject3 : OltPersonName
    {
        public static BasicAdapterObject3 FakerData()
        {
            var result = new BasicAdapterObject3
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<BasicAdapterObject3> FakerList(int number)
        {
            var list = new List<BasicAdapterObject3>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
