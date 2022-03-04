using System.Collections.Generic;

namespace OLT.EF.Common.Tests.Assets.Models
{
    public class PersonEntity
    {
        public string NameFirst { get; set; }
        public string NameLast { get; set; }

        public static List<PersonEntity> FakerList(int number)
        {
            var list = new List<PersonEntity>();

            for (int i = 0; i < number; i++)
            {
                list.Add(new PersonEntity
                {
                    NameFirst = Faker.Name.First(),
                    NameLast = Faker.Name.Last(),
                });
            }

            return list;
        }
    }
}
