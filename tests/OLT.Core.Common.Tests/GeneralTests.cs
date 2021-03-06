using OLT.Constants;
using OLT.Core.Common.Tests.Assets;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests
{

    public class GeneralTests
    {
        private const string NullString = null;
        private const string EmptyString = "";
        private const string WhitespaceString = " ";

        [Fact]
        public void OltDisposable()
        {
            var obj = new TestDisposable();
            Assert.NotNull(obj);
            Assert.False(obj.IsDisposed());
            obj.Dispose();
            Assert.True(obj.IsDisposed());

            using (var obj2 = new TestDisposable())
            {
                Assert.NotNull(obj2);
                Assert.False(obj2.IsDisposed());                
            }
        }

        [Fact]
        public void OltRequest()
        {
            var model = new TestPersonModel
            {
                Name = Faker.Name.FullName(),
                StreetAddress = Faker.Address.StreetAddress()
            };

            var request = new OltRequest<TestPersonModel>(model);

            Assert.NotNull(request.Value);
            Assert.IsType<TestPersonModel>(request.Value);
            Assert.Equal(model.Name, request.Value.Name);
            Assert.Equal(model.StreetAddress, request.Value.StreetAddress);

            request = new OltRequest<TestPersonModel>(null);
            Assert.Null(request.Value);

            var stringRequest = new OltRequest<string>(null);
            Assert.Null(stringRequest.Value);

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


    }
}