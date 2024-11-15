using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts.Entities;
using System.ComponentModel;
using Testcontainers.MsSql;

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests;

public class OltSqlDbContextTests : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;

    public OltSqlDbContextTests()
    {
        //var config = new MsSqlConfiguration("OltSqlDbContextTest");

        _msSqlContainer = new MsSqlBuilder()            
            .WithPassword("yourStrong(!)Password")            
            .WithPortBinding("1433", "1673")            
            .Build();
    }

    private string GetConnectionString()
    {
        return new SqlConnectionStringBuilder(_msSqlContainer.GetConnectionString())
        {
            InitialCatalog = "MyTestDbData"
        }
        .ToString();
    }

    private DbContextOptions<TestSqlContext> GetContextOptions()
    {
        var connStr = GetConnectionString();
        return new DbContextOptionsBuilder<TestSqlContext>().UseSqlServer(connStr).Options;
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        using (var context = new TestSqlContext(GetContextOptions()))
        {
            await context.Database.EnsureCreatedAsync();
        }

    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task OltSqlDbContext_ShouldSetIdentityColumnsForIOltEntityId()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IOltDbAuditUser, TestDbAuditUser>()
            .AddDbContext<TestSqlContext>(options => options.UseSqlServer(GetConnectionString()))
            .BuildServiceProvider();

        var dbContext = serviceProvider.GetRequiredService<TestSqlContext>();

        Assert.NotNull(dbContext);

        var expected = TestSqlContext.Identity_Seed;
        expected = await AddRecord(dbContext, expected);
        expected = await AddRecord(dbContext, expected + TestSqlContext.Identity_Increment);
        expected = await AddRecord(dbContext, expected + TestSqlContext.Identity_Increment);


        var columns = dbContext.Database.SqlQuery<Columns>($"SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS").ToList();

        
        var entity = columns.FirstOrDefault(p => p.TABLE_NAME == "TestEntities");

    }

    private async Task<int> AddRecord(TestSqlContext context, int expected)
    {
        var entity = new TestEntity();
        await context.TestEntities.AddAsync(entity);
        await context.SaveChangesAsync();
        Assert.Equal(expected, entity.Id);
        return entity.Id;

    }

    public class TestDbAuditUser : IOltDbAuditUser
    {
        public string? GetDbUsername()
        {
            return Faker.Name.FullName();
        }
    }

    public class Columns
    {
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
    }
}
