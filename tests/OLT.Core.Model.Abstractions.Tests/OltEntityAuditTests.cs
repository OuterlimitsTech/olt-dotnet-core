namespace OLT.Core.Model.Abstractions.Tests;

public class OltEntityAuditTests
{
    private class TestOltEntityAudit : OltEntityAudit { }

    [Fact]
    public void CreateDate_ShouldBeInitializedToUtcNow()
    {
        // Arrange & Act
        var entity = new TestOltEntityAudit();

        // Assert
        Assert.True((DateTimeOffset.UtcNow - entity.CreateDate).TotalSeconds < 1);
    }

    [Fact]
    public void CreateUser_ShouldBeInitializedToUnknownCreateUser()
    {
        // Arrange & Act
        var entity = new TestOltEntityAudit();

        // Assert
        Assert.Equal(Constants.OltCommonDefaults.UnknownCreateUser, entity.CreateUser);
    }

    [Fact]
    public void ModifyDate_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityAudit();

        // Assert
        Assert.Null(entity.ModifyDate);
    }

    [Fact]
    public void ModifyUser_ShouldBeNullByDefault()
    {
        // Arrange & Act
        var entity = new TestOltEntityAudit();

        // Assert
        Assert.Null(entity.ModifyUser);
    }

}
