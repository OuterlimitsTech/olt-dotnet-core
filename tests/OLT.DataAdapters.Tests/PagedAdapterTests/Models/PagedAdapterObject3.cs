using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.PagedAdapterTests.Models
{
    public class PagedAdapterObject3 : OltPersonName
    {
        public static PagedAdapterObject3 FakerData()
        {
            var result = new PagedAdapterObject3
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<PagedAdapterObject3> FakerList(int number)
        {
            var list = new List<PagedAdapterObject3>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }
}
