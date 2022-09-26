using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Searchers.Tests.Assets
{
    public class FakeEntity : IOltEntity
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int SelectValue { get; set; }
        public int? NullableInt { get; set; }

        public DateTimeOffset SomeDate { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public static FakeEntity FakerData()
        {
            return new FakeEntity
            {
                Id = Faker.RandomNumber.Next(1000, 900000),
                UniqueId = Guid.NewGuid(),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                SelectValue = Faker.RandomNumber.Next(1000, 900000),
                NullableInt = Faker.RandomNumber.Next(1000, 900000),
                SomeDate = DateTimeOffset.Now.AddHours(Faker.RandomNumber.Next(-150, 150))
            };
        }

        public static List<FakeEntity> FakerList(int number)
        {
            var list = new List<FakeEntity>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData();
                list.Add(item);
            }
            return list;
        }

        public static FakeEntity FakerData(List<int> selectValues, bool deleted = false)
        {
            var result = new FakeEntity
            {
                Id = Faker.RandomNumber.Next(1000, 900000),
                UniqueId = Guid.NewGuid(),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                SelectValue = selectValues.OrderBy(p => Guid.NewGuid()).FirstOrDefault(),
                NullableInt = Faker.RandomNumber.Next(1000, 900000),
                SomeDate = DateTimeOffset.Now.AddHours(Faker.RandomNumber.Next(-150, 150))
            };

            if (deleted)
            {
                result.DeletedOn = DateTimeOffset.Now.AddMinutes(Faker.RandomNumber.Next(10, 1000) * -1);
                result.DeletedBy = Faker.Internet.UserName();
            }
            return result;
        }

        public static List<FakeEntity> FakerList(int number, List<int> selectValues, bool deleted = false)
        {
            var list = new List<FakeEntity>();
            for (int i = 0; i < number; i++)
            {
                var item = FakerData(selectValues, deleted);
                list.Add(item);
            }
            return list;
        }

    }
}


