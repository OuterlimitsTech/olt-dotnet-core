namespace OLT.Core.Model.Abstractions.Tests;

public class OltEntityIdDeletableTests
{
    private class TestOltEntityIdDeletable : OltEntityIdDeletable
    {
        public static TestOltEntityIdDeletable FakerData()
        {
            return new TestOltEntityIdDeletable
            {
                Id = Faker.RandomNumber.Next(1000, 10000),
                DeletedBy = Faker.Internet.UserName(),
                DeletedOn = TestHelper.FakerDateTimePast(),
                CreateUser = Faker.Internet.UserName(),
                CreateDate = TestHelper.FakerDateTimePast(),
                ModifyUser = Faker.Internet.UserName(),
                ModifyDate = TestHelper.FakerDateTimePast(),
            };
        }
    }

    [Fact]
    public void Id_ShouldBeInitializedToDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Equal(0, entity.Id);
    }

    [Fact]
    public void CreateDate_ShouldBeInitializedToUtcNow()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.True((DateTimeOffset.UtcNow - entity.CreateDate).TotalSeconds < 1);
    }

    [Fact]
    public void CreateUser_ShouldBeInitializedToUnknownCreateUser()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Equal(Constants.OltCommonDefaults.UnknownCreateUser, entity.CreateUser);
    }

    [Fact]
    public void ModifyDate_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Null(entity.ModifyDate);
    }

    [Fact]
    public void ModifyUser_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Null(entity.ModifyUser);
    }

    [Fact]
    public void DeletedOn_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Null(entity.DeletedOn);
    }

    [Fact]
    public void DeletedBy_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityIdDeletable();

        // Assert
        Assert.Null(entity.DeletedBy);
    }   

    [Fact]
    public void Model_ShouldBeDefaults()
    {
        var model = new TestOltEntityIdDeletable();
        Assert.Equal(0, model.Id);
        Assert.True(model.CreateDate < DateTimeOffset.UtcNow.AddSeconds(2));
        Assert.True(model.CreateDate > DateTimeOffset.UtcNow.AddSeconds(-2));
        Assert.Equal(OLT.Constants.OltCommonDefaults.UnknownCreateUser, model.CreateUser);
        Assert.Null(model.ModifyDate);
        Assert.Null(model.ModifyUser);
        Assert.Null(model.DeletedBy);
        Assert.Null(model.DeletedOn);
    }

    [Fact]
    public void Model_ShouldNoteBeDefaults()
    {
        var model = TestOltEntityIdDeletable.FakerData();
        Assert.NotEqual(0, model.Id);
        Assert.NotEqual(DateTimeOffset.MinValue, model.CreateDate);
        Assert.True(model.CreateDate < DateTimeOffset.UtcNow);
        Assert.NotEqual(OLT.Constants.OltCommonDefaults.UnknownCreateUser, model.CreateUser);
        Assert.NotNull(model.ModifyDate);
        Assert.NotNull(model.ModifyUser);
        Assert.NotNull(model.DeletedBy);
        Assert.NotNull(model.DeletedOn);
    }

    [Fact]
    public void Model_ShouldBeAssignableToInterfaces()
    {
        var model = TestOltEntityIdDeletable.FakerData();
        Assert.IsAssignableFrom<IOltEntity>(model);
        Assert.IsAssignableFrom<IOltEntityId>(model);
        Assert.IsAssignableFrom<IOltEntityAudit>(model);
        Assert.IsAssignableFrom<IOltEntityDeletable>(model);
    }

}
