using OLT.Core.Common.Tests.Assets;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class UnitTests
    {
        private const string NullString = null;
        private const string EmptyString = "";
        private const string WhitespaceString = " ";

        [Fact]
        public void OltDisposable()
        {
            using(var obj = new TestDisposable())
            {
                Assert.NotNull(obj);
                Assert.False(obj.IsDeposed());
                obj.Dispose();
                Assert.True(obj.IsDeposed());
            }
        }


        [Fact]
        public void OltResult()
        {
            Assert.True(new OltResultSuccess().Success);

            var resultValid = new OltResultValid();
            Assert.True(resultValid.Success);
            Assert.False(resultValid.Invalid);
            Assert.Empty(resultValid.Results);

            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(NullString));
            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(EmptyString));
            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(WhitespaceString));

            var resultValidation = new OltResultValidation();
            Assert.True(resultValidation.Success);
            Assert.False(resultValidation.Invalid);
            Assert.Empty(resultValidation.Results);

            var expected = Faker.Lorem.Sentence();
            resultValidation = new OltResultValidation(expected);

            Assert.False(resultValidation.Success);
            Assert.True(resultValidation.Invalid);
            Assert.NotEmpty(resultValidation.Results);

            Assert.Equal(expected, resultValidation.Results[0]?.Message);

        }

        [Fact]
        public void OltValidationError()
        {
            var expected = Faker.Lorem.Sentence();
            Assert.Throws<System.ArgumentNullException>(() => new OltValidationError(NullString));
            Assert.Throws<System.ArgumentNullException>(() => new OltValidationError(EmptyString));
            Assert.Throws<System.ArgumentNullException>(() => new OltValidationError(WhitespaceString));

            var error = new OltValidationError(expected);
            Assert.Equal(expected, error.Message);

            error.Message = Faker.Lorem.Sentence();
            Assert.NotEqual(expected, error.Message);

            
            Assert.Throws<System.ArgumentNullException>(() => error.Message = NullString);
            Assert.Throws<System.ArgumentNullException>(() => error.Message = EmptyString);
            Assert.Throws<System.ArgumentNullException>(() => error.Message = WhitespaceString);

        }


    }
}