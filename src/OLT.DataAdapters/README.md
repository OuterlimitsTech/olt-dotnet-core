[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## OLT Adapter Resolver and adapters for IQueryable and paged results

```csharp

// Inject IOltAdapterResolver 


// Checks to see if can project IQueryable
adapterResolver.CanProjectTo<PersonEntity, PersonModel>();  

//Simple Map
var person = adapterResolver.Map<PersonEntity, PersonModel>(entity, new PersonModel());


var queryable = Context.People.GetAll();
var records = adapterResolver.projectTo<PersonEntity, PersonModel>(queryable);

```

#### Simple Adapater

```csharp

public class PersonEntityToPersonModelAdapter : OltAdapter<PersonEntity, PersonModel>
{
	public override void Map(PersonEntity source, PersonModel destination)
	{
		destination.Name = new OltPersonName
		{
			First = source.FirstName,
			Last = source.LastName,
		};
	}

	public override void Map(PersonModel source, PersonEntity destination)
	{
		destination.FirstName = source.Name.First;
		destination.LastName = source.Name.Last;
	}
}

```


#### Projection Adapter using IQueryable

```csharp

public class PersonEntityToPersonModelQueryableAdapter : OltAdapter<PersonEntity, PersonModel>, IOltAdapterQueryable<PersonEntity, PersonModel>
{
	public PersonEntityToPersonModelQueryableAdapter()
	{
		this.WithOrderBy(p => p.OrderBy(o => o.FirstName).ThenBy(o => o.LastName));
	}

	public override void Map(PersonEntity source, PersonModel destination)
	{
		destination.Name = new OltPersonName
		{
			First = source.FirstName,
			Last = source.LastName,
		};
	}

	public override void Map(PersonModel source, PersonEntity destination)
	{
		destination.FirstName = source.Name.First;
		destination.LastName = source.Name.Last;
	}

	public IQueryable<PersonModel> Map(IQueryable<PersonEntity> queryable)
	{
		return queryable.Select(entity => new PersonModel
		{
			Name = new OltPersonName
			{
				First = entity.FirstName,
				Last = entity.LastName,
			}
		});
	}
}


```

#### Projection Adapter using IQueryable for Paging (requires Default Order By)

```csharp 

public class PersonEntityToPersonModelPagedAdapter : OltAdapterPaged<PersonEntity, PersonModel>
{
	public override void Map(PersonEntity source, PersonModel destination)
	{
		destination.Name = new OltPersonName
		{
		  First = source.FirstName,
		  Last = source.LastName,
		};
	}

	public override void Map(PersonModel source, PersonEntity destination)
	{
		destination.FirstName = source.Name?.First;
	    destination.LastName = source.Name?.Last;
	}

	public override IQueryable<PersonModel> Map(IQueryable<PersonEntity> queryable)
	{
		return queryable.Select(entity => new PersonModel
		{
			Name = new OltPersonName
			{
				First = entity.FirstName,
				Last = entity.LastName,
			}
		});
	}

	public override IOrderedQueryable<PersonEntity> DefaultOrderBy(IQueryable<PersonEntity> queryable)
	{
		return queryable.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
	}
}

```