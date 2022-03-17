using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace OLT.DataAdapters.Tests
{
    public class ExceptionTests
    {
        private const string DefaultMessage = "Test Error";

        private T ToSerialize<T>(T obj)
        {
            using Stream s = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(s, obj);
            s.Position = 0; // Reset stream position
            return (T)formatter.Deserialize(s);
        }

        [Fact]
        public void AdapterNotFoundException()
        {
            var ex = new OltAdapterNotFoundException(DefaultMessage);
            Assert.Equal($"Adapter Not Found {DefaultMessage}", ex.Message);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }   

        [Fact]
        public void AdapterNotFoundExceptionTyped()
        {
            var ex = new OltAdapterNotFoundException<QueryableAdapterObject1, QueryableAdapterObject2>();
            Assert.Equal($"Adapter Not Found {typeof(QueryableAdapterObject1).FullName} -> {typeof(QueryableAdapterObject2).FullName}", ex.Message);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }
    }
}