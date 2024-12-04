using Xunit.Sdk;

namespace OLT.Core.Model.Abstractions.Tests;

public class OltEntityIdTests
{   

    [Fact]
    public void Id_ShouldBeInitializedToDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityId();

        // Assert
        Assert.Equal(0, entity.Id);
    }

    [Fact]
    public void CreateDate_ShouldBeInitializedToUtcNow()
    {
        // Arrange & Act
        var entity = new TestOltEntityId();

        // Assert
        Assert.True((DateTimeOffset.UtcNow - entity.CreateDate).TotalSeconds < 1);
    }

    [Fact]
    public void CreateUser_ShouldBeInitializedToUnknownCreateUser()
    {
        // Arrange & Act
        var entity = new TestOltEntityId();

        // Assert
        Assert.Equal(Constants.OltCommonDefaults.UnknownCreateUser, entity.CreateUser);
    }

    [Fact]
    public void ModifyDate_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityId();

        // Assert
        Assert.Null(entity.ModifyDate);
    }

    [Fact]
    public void ModifyUser_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityId();

        // Assert
        Assert.Null(entity.ModifyUser);
    }

    [Fact]
    public void Model_ShouldBeDefaults()
    {
        var model = new TestOltEntityId();
        Assert.Equal(0, model.Id);
        Assert.True(model.CreateDate < DateTimeOffset.UtcNow.AddSeconds(2));
        Assert.True(model.CreateDate > DateTimeOffset.UtcNow.AddSeconds(-2));
        Assert.Equal(OLT.Constants.OltCommonDefaults.UnknownCreateUser, model.CreateUser);
        Assert.Null(model.ModifyDate);
        Assert.Null(model.ModifyUser);
    }

    [Fact]
    public void Model_ShouldNoteBeDefaults()
    {
        var model = TestOltEntityId.FakerData();
        Assert.NotEqual(0, model.Id);
        Assert.NotEqual(DateTimeOffset.MinValue, model.CreateDate);
        Assert.True(model.CreateDate < DateTimeOffset.Now);
        Assert.NotEqual(OLT.Constants.OltCommonDefaults.UnknownCreateUser, model.CreateUser);
        Assert.NotNull(model.ModifyDate);
        Assert.NotNull(model.ModifyUser);
    }

    [Fact]
    public void Model_ShouldBeAssignableToInterfaces()
    {
        var model = TestOltEntityId.FakerData();
        Assert.IsAssignableFrom<IOltEntity>(model);
        Assert.IsAssignableFrom<IOltEntityId>(model);
        Assert.IsAssignableFrom<IOltEntityAudit>(model);
        Assert.Throws<IsAssignableFromException>(() => Assert.IsAssignableFrom<IOltEntityDeletable>(model));
    }

    private class TestOltEntityId : OltEntityId
    {
        public static TestOltEntityId FakerData()
        {
            return new TestOltEntityId
            {
                Id = Faker.RandomNumber.Next(1000, 10000),
                CreateUser = Faker.Internet.UserName(),
                CreateDate = TestHelper.FakerDateTimePast(),
                ModifyUser = Faker.Internet.UserName(),
                ModifyDate = TestHelper.FakerDateTimePast(),
            };
        }
    }

}
