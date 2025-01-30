[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Entity Framework Core for SQL Server 

- Sets the UseIdentityColumn for Entities that implememnt IOltEntityId 
- Full Text Search (FTS) Utility Helper class



#### OltSqlDbContext

By inheriting the OltSqlDbContext, it will automatically call the OltSqlModelBuilderExtensions.SetIdentityColumns

```csharp
public class DatabaseContext : OltSqlDbContext<DatabaseContext>
{
	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{
	}
	
	public override string DefaultSchema => "dbo";
    public override bool DisableCascadeDeleteConvention => true;
    public override OltContextStringTypes DefaultStringType => OltContextStringTypes.Varchar;
    public override bool ApplyGlobalDeleteFilter => true;
    protected override int IdentitySeed => 1000;
    protected override int IdentityIncrement => 2;

	...
	
	public virtual DbSet<MyEntity> MyEntities => Set<MyEntity>();

	...
}
```

#### OltSqlModelBuilderExtensions

```csharp
OltSqlModelBuilderExtensions.SetIdentityColumns(modelBuilder, identitySeed, identityIncrement)
```


#### OltFullTextSearchUtil 
_Full Text Search Helper_

```csharp

public IQueryable<TEntity> BuildQueryable(IQueryable<TEntity> queryable)
{
   if (Name.First.IsNotEmpty())
   {
	   var firstName = OltFullTextSearchUtil.Contains(Name.First, OltFtsWildCardType.BeginsWith, true);
	   queryable = queryable.Where(p => EF.Functions.Contains(p.FirstName, firstName));
   }

   if (Name.Middle.IsNotEmpty())
   {
	   var middleName = OltFullTextSearchUtil.Contains(Name.Middle, OltFtsWildCardType.BeginsWith, true);
	   queryable = queryable.Where(p => EF.Functions.Contains(p.MiddleName, middleName));
   }

   if (Name.Last.IsNotEmpty())
   {
	   var lastName = OltFullTextSearchUtil.Contains(Name.Last, OltFtsWildCardType.BeginsWith, true);
	   queryable = queryable.Where(p => EF.Functions.Contains(p.LastName, lastName));
   }

   return base.BuildQueryable(queryable);
}


```