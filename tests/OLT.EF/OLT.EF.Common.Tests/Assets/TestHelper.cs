using OLT.EF.Common.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.EF.Common.Tests.Assets
{
    public static class TestHelper
    {
        public static DateTimeOffset FakerDateTimePast()
        {
            var milliseconds = Faker.RandomNumber.Next(10000, 100000) * -1;
            return DateTimeOffset.Now.AddMilliseconds(milliseconds);
        }

        public static List<EntityPersonModel> BuildTestList(EntityPersonModel expected, int startingId = 1000, int minMix = 4, int maxMix = 8)
        {
            return BuildTestList(new List<EntityPersonModel> { expected }, startingId, minMix, maxMix);
        }


        /// <summary>
        /// Randomize the List
        /// </summary>
        /// <param name="expected">Expected list of data to randomly add to results</param>
        /// <param name="startingId"></param>
        /// <param name="minMix"></param>
        /// <param name="maxMix"></param>
        /// <returns></returns>
        public static List<EntityPersonModel> BuildTestList(List<EntityPersonModel> expected, int startingId = 1000, int minMix = 0, int maxMix = 6)
        {
            var randomized = expected.OrderBy(x => Guid.NewGuid()).ToList();
            var list = new List<EntityPersonModel>();
            for (int i = 0; i < randomized.Count; i++)
            {
                var rangeList = EntityPersonModel.FakerList(Faker.RandomNumber.Next(minMix, maxMix));
                rangeList.ForEach(item =>
                {
                    item.Id = startingId;
                    startingId++;
                    list.Add(item);
                });

                randomized[i].Id = startingId;
                startingId++;
                list.Add(randomized[i]);
            }
            return list;
        }


        public static void SetFirstName(this List<EntityPersonModel> list, string firstName)
        {
            list.ForEach(person => person.SetFirstName(firstName));
        }

        public static void SetFirstName(this EntityPersonModel person, string firstName)
        {
            person.FirstName = firstName;
        }

        public static void SetFirstNameStartsWith(this List<EntityPersonModel> list, string firstName)
        {
            list.ForEach(person => person.SetFirstNameStartsWith(firstName));
        }

        public static void SetFirstNameStartsWith(this EntityPersonModel person, string firstName)
        {
            person.FirstName = $"{firstName}{person.FirstName}";
        }

        public static void SetFirstNameEndsWith(this List<EntityPersonModel> list, string firstName)
        {
            list.ForEach(person => person.SetFirstNameEndsWith(firstName));
        }

        public static void SetFirstNameEndsWith(this EntityPersonModel person, string firstName)
        {
            person.FirstName = $"{person.FirstName}{firstName}";
        }

        public static void SetFirstNameContains(this List<EntityPersonModel> list, string firstName)
        {
            list.ForEach(person => person.SetFirstNameContains(firstName));
        }

        public static void SetFirstNameContains(this EntityPersonModel person, string firstName)
        {
            person.FirstName = $"{person.FirstName}{firstName}{Faker.Name.Last()}";
        }



        public static void SetLastNameStartsWith(this List<EntityPersonModel> list, string lastName)
        {
            list.ForEach(person => person.SetLastNameStartsWith(lastName));
        }

        public static void SetLastNameStartsWith(this EntityPersonModel person, string lastName)
        {
            person.LastName = $"{lastName}{person.LastName}";
        }

    }
}
