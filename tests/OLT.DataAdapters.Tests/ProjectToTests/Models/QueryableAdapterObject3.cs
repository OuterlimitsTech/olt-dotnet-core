using OLT.Core;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.ProjectToTests.Models
{
    public class QueryableAdapterObject3 : OltPersonName
    {
        public static QueryableAdapterObject3 FakerData()
        {
            var result = new QueryableAdapterObject3
            {
                First = Faker.Name.First(),
                Last = Faker.Name.Last(),
            };
            return result;
        }

        public static List<QueryableAdapterObject3> FakerList(int number)
        {
            var list = new List<QueryableAdapterObject3>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }

}
