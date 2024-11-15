using Microsoft.EntityFrameworkCore;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts.Entities;
using Testcontainers.MsSql;

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests;

public class OltSqlModelBuilderExtensionsTests : IAsyncLifetime
{
    public const int Identity_Seed = 100;
    public const int Identity_Increment = 5;

    private readonly MsSqlContainer _msSqlContainer;

    public OltSqlModelBuilderExtensionsTests()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPassword("yourStrong(!)Password")
            .Build();
    }

    private DbContextOptions<TestDbContext> GetContextOptions()
    {
        return new DbContextOptionsBuilder<TestDbContext>().UseSqlServer(_msSqlContainer.GetConnectionString()).Options;
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        using (var context = new TestDbContext(GetContextOptions()))
        {
            await context.Database.EnsureCreatedAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task SetIdentityColumns_ShouldSetIdentityColumnsForIOltEntityId()
    {
        var expected = Identity_Seed;
        var options = GetContextOptions();
        using (var context = new TestDbContext(options))
        {
            expected = await AddRecord(context, expected);
            expected = await AddRecord(context, expected + Identity_Increment);
            expected = await AddRecord(context, expected + Identity_Increment);
        }  

    }

    private async Task<int> AddRecord(TestDbContext context, int expected)
    {
        var entity = new TestEntity();
        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        Assert.Equal(expected, entity.Id);
        return entity.Id;        

    }


}
