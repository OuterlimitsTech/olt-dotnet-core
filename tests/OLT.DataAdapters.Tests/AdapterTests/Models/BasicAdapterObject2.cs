using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.AdapterTests.Models
{
    public class BasicAdapterObject2 : BaseAdapterObject2
    {

        public static BasicAdapterObject2 FakerData()
        {
            var result = new BasicAdapterObject2
            {
                Name = new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last(),
                }
            };


            return result;
        }

        public static List<BasicAdapterObject2> FakerList(int number)
        {
            var list = new List<BasicAdapterObject2>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
