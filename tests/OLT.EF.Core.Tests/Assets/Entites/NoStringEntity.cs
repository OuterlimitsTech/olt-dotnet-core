using OLT.Core;

namespace OLT.EF.Core.Tests.Assets.Entites
{
    /// <summary>
    /// Used to GetNullableStringPropertyMetaData where the entity has no strings
    /// </summary>
    public class NoStringEntity : IOltEntityId<int>
    {
        public int Id { get; set; }
        public long LongValue { get; set; }
        public int IntValue { get; set; }
        public short ShortValue { get; set; }
        

        public static NoStringEntity FakerEntity()
        {
            return new NoStringEntity
            {
                LongValue = Faker.RandomNumber.Next(30000, 50000),
                IntValue = Faker.RandomNumber.Next(5000, 7000),
                ShortValue = (short)Faker.RandomNumber.Next(1000, 2000),
            };
        }
    }

}
