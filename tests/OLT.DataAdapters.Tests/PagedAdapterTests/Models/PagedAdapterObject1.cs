using OLT.DataAdapters.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Models
{
    public class PagedAdapterObject1 : BaseAdapterObject1
    {
        public static PagedAdapterObject1 FakerData()
        {
            var result = new PagedAdapterObject1
            {
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
            };
            return result;
        }

        public static List<PagedAdapterObject1> FakerList(int number)
        {
            var list = new List<PagedAdapterObject1>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
