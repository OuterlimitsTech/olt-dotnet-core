using FluentAssertions;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using OLT.Core.CommandBus.Tests.Assets.Models;
using System;
using Xunit;

namespace OLT.Core.CommandBus.Tests
{
    public class OltCommandResultUnitTest
    {

        [Fact]
        public void ExceptionTests()
        {
            var nullCommandResult = OltCommandResult.Complete();            
            var addressEntityResult = OltCommandResult.Complete(new TestPersonDto());
            Assert.Throws<OltCommandResultNullException>(() => nullCommandResult.ToResult<UserEntity>());
            Assert.Throws<InvalidCastException>(() => addressEntityResult.ToResult<UserEntity>());
        }

        [Fact]
        public void ToResultTests()
        {
            var entity = UserEntity.FakerEntity();
            var commandResult = OltCommandResult.Complete(entity);
            var result = commandResult.ToResult<UserEntity>();
            result.Should().BeEquivalentTo(entity);            
        }

    }

}