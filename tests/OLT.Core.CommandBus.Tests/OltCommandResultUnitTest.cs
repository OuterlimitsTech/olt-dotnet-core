using AwesomeAssertions;
using OLT.Core.CommandBus.Tests.Assets.EfCore.Entites;
using OLT.Core.CommandBus.Tests.Assets.Models;
using OLT.Core.CommandBus.Tests.Assets.Validators;
using System;
using System.Linq;
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
            Assert.Throws<NullReferenceException>(() => nullCommandResult.ToResult<UserEntity>());
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


        [Fact]
        public void ValidationResultTests()
        {
            var validator = new TestPersonDtoValidator();
            var dto = new TestPersonDto();
            var errors = validator.Validate(dto).Errors.Select(s => s.ErrorMessage).ToList();
            var result = OltCommandValidationResult.FromResult(validator.Validate(dto), validator.Validate(dto));
            result.Errors.Should().HaveCount(6);  //Force validator twice to test params
            result.Valid.Should().Be(false);

            result.ToException().Results.Select(s => s.Message).Should().Contain(errors);

            OltCommandValidationResult.DontAllow(errors).Errors.Should().HaveCount(3);
            OltCommandValidationResult.DontAllow(errors).Valid.Should().Be(false);

            OltCommandValidationResult.Allow().Errors.Should().HaveCount(0);
            OltCommandValidationResult.Allow().Valid.Should().Be(true);
        }

    }

}