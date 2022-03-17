using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using System.Collections.Generic;

namespace OLT.DataAdapters.Tests.ProjectToTests.Models
{
    public class QueryableAdapterObject2 : BaseAdapterObject2
    {

        public static QueryableAdapterObject2 FakerData()
        {
            var result = new QueryableAdapterObject2
            {
                Name = new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last(),
                }
            };


            return result;
        }

        public static List<QueryableAdapterObject2> FakerList(int number)
        {
            var list = new List<QueryableAdapterObject2>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }

}
