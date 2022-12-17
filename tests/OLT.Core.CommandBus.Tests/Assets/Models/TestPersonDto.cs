namespace OLT.Core.CommandBus.Tests.Assets.Models
{

    public class TestPersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public static TestPersonDto FakerDto()
        {
            return new TestPersonDto
            {
                Id = Faker.RandomNumber.Next(1000, 50000),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last()
            };
        }
    }
}
