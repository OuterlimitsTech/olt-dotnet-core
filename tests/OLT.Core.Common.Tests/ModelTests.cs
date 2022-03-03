using Xunit;

namespace OLT.Core.Common.Tests
{
    public class ModelTests
    {
        [Theory]
        [InlineData("Test Jones", "Test", null, "Jones", null)]
        [InlineData("Test M Jones", "Test", "M", "Jones", null)]
        [InlineData("Test M Jones Jr", "Test", "M", "Jones", "Jr")]
        [InlineData("M Jones Jr", null, "M", "Jones", "Jr")]
        [InlineData("M Jr", null, "M", null, "Jr")]
        [InlineData("", null, null, null, null)]
        public void PersonNameTest(string expected, string first, string middle, string last, string suffix)
        {
            var model = new OltPersonName
            {
                First = first,
                Middle = middle,
                Last = last,
                Suffix = suffix
            };            

            Assert.Equal(first, model.First);
            Assert.Equal(middle, model.Middle);
            Assert.Equal(last, model.Last);
            Assert.Equal(suffix, model.Suffix);
            Assert.Equal(expected, model.FullName);
            Assert.NotNull(model as IOltPersonName);
        }


        [Fact]
        public void PagingParamsTest()
        {
            var model = new OltPagingParams();
            Assert.Equal(1, model.Page);
            Assert.Equal(10, model.Size);
            Assert.NotNull(model as IOltPagingParams);
            Assert.NotNull(model as IOltPaged);            

            var page = Faker.RandomNumber.Next();
            var size = Faker.RandomNumber.Next();
            model.Page = page;
            model.Size = size;

            Assert.Equal(page, model.Page);
            Assert.Equal(size, model.Size);
            
        }
    }
}

