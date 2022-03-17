using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.AdapterTests.Models
{
    public class BasicAdapterObject1 : BaseAdapterObject1
    {
        public static BasicAdapterObject1 FakerData()
        {
            var result = new BasicAdapterObject1
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            return result;
        }

        public static List<BasicAdapterObject1> FakerList(int number)
        {
            var list = new List<BasicAdapterObject1>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
