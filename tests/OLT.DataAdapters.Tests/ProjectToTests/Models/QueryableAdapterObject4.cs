using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.ProjectToTests.Models
{
    public class QueryableAdapterObject4 : OltPersonName
    {
        public static QueryableAdapterObject4 FakerData()
        {
            var result = new QueryableAdapterObject4
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<QueryableAdapterObject4> FakerList(int number)
        {
            var list = new List<QueryableAdapterObject4>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }

}
