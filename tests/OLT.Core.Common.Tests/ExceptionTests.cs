using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Common.Tests
{
    public class ExceptionTests
    {
        private enum RecordNotFound
        {
            [Description("Person")]
            Person
        }

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
        public void ExceptionTest()
        {
            var ex = new OltException(DefaultMessage);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }

        [Fact]
        public void InnerExceptionTest()
        {
            var innerException = new Exception($"Inner {DefaultMessage}");
            var ex = new OltException(DefaultMessage, innerException);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }

        [Fact]
        public void BadRequestExceptionTest()
        {
            var ex = new OltBadRequestException(DefaultMessage);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }

        [Fact]
        public void RecordNotFoundException()
        {
            var ex = new OltRecordNotFoundException(DefaultMessage);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }

        [Fact]
        public void RecordNotFoundExceptionEnum()
        {
            var ex = new OltRecordNotFoundException<RecordNotFound>(RecordNotFound.Person);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }

        [Fact]
        public void ValidationExceptionTest()
        {
            var list = new List<OltValidationError> { new OltValidationError(DefaultMessage) };
            var ex = new OltValidationException(list);
            var result = ToSerialize(ex);
            Assert.Equal(ex.Message, result.Message);
        }
    }
}
