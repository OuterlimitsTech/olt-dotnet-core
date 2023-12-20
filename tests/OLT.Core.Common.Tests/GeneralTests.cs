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


    }
}