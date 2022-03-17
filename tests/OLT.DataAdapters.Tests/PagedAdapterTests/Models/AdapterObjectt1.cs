using OLT.Core;
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

    public class PagedAdapterObject2 : BaseAdapterObject2
    {

        public static PagedAdapterObject2 FakerData()
        {
            var result = new PagedAdapterObject2
            {
                Name = new OltPersonName
                {
                    First = Faker.Name.First(),
                    Last = Faker.Name.Last(),
                }
            };


            return result;
        }

        public static List<PagedAdapterObject2> FakerList(int number)
        {
            var list = new List<PagedAdapterObject2>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }
    }

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
