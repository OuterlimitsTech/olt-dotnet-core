using OLT.Core.Common.Tests.Assets;
using System.Linq;
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
        public void OltRequest()
        {
            var model = new TestModel
            {
                Name = Faker.Name.FullName(),
                StreetAddress = Faker.Address.StreetAddress()
            };

            var request = new OltRequest<TestModel>(model);

            Assert.NotNull(request.Value);
            Assert.IsType<TestModel>(request.Value);
            Assert.Equal(model.Name, request.Value.Name);
            Assert.Equal(model.StreetAddress, request.Value.StreetAddress);

            request = new OltRequest<TestModel>(null);
            Assert.Null(request.Value);

            var stringRequest = new OltRequest<string>(null);
            Assert.Null(request.Value);

            stringRequest = new OltRequest<string>(model.Name);
            Assert.NotNull(stringRequest.Value);
            Assert.IsType<string>(stringRequest.Value);
            Assert.Equal(model.Name, stringRequest.Value);


            var expectedNumber = Faker.RandomNumber.Next();
            var numberRequest = new OltRequest<int?>(null);
            Assert.Null(numberRequest.Value);            
            numberRequest = new OltRequest<int?>(expectedNumber);
            Assert.NotNull(numberRequest.Value);
            Assert.IsType<int>(numberRequest.Value);
            Assert.Equal(expectedNumber, numberRequest.Value);

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

            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(new OltValidationError(NullString)));
            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(new OltValidationError(EmptyString)));
            Assert.Throws<System.ArgumentNullException>(() => new OltResultValidation(new OltValidationError(WhitespaceString)));

            resultValidation = new OltResultValidation(new OltValidationError(expected));
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

        [Fact]
        public void OltDefaultCharacter()
        {
            Assert.Equal(26, OltDefaults.Characters.UpperCase.Length);
            Assert.Equal(26, OltDefaults.Characters.UpperCase.ToList().Distinct().Count());

            Assert.Equal(26, OltDefaults.Characters.LowerCase.Length);
            Assert.Equal(26, OltDefaults.Characters.LowerCase.ToList().Distinct().Count());

            Assert.Equal(10, OltDefaults.Characters.Numerals.Length);
            Assert.Equal(10, OltDefaults.Characters.Numerals.ToList().Distinct().Count());

            Assert.Equal(29, OltDefaults.Characters.Symbols.Length);
            Assert.Equal(29, OltDefaults.Characters.Symbols.ToList().Distinct().Count());

            Assert.Equal(8, OltDefaults.Characters.Special.Length);
            Assert.Equal(8, OltDefaults.Characters.Special.ToList().Distinct().Count());

        }

    }
}