using System;
using Xunit;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using AwesomeAssertions;
using OLT.Core.CommandBus.Tests.Assets.Models;

namespace OLT.Core.CommandBus.Tests
{
    public class CommandExtensionsUnitTest
    {
        
        [Fact]
        public void ToResultTests()
        {
            var entity = UserEntity.FakerEntity();
            var commandResult = OltCommandResult.Complete(entity);

            OltCommandExtensions.ToResult<UserEntity>(commandResult).Should().BeEquivalentTo(entity);
            OltCommandExtensions.ToResult<UserEntity>((IOltCommandResult)commandResult).Should().BeEquivalentTo(entity);
        }

        [Fact]
        public void ExceptionTests()
        {
            Assert.Throws<NullReferenceException>(() => OltCommandExtensions.ToResult<UserEntity>(OltCommandResult.Complete()));
            Assert.Throws<InvalidCastException>(() => OltCommandExtensions.ToResult<UserEntity>(OltCommandResult.Complete(new TestPersonDto())));
        }
    }
}