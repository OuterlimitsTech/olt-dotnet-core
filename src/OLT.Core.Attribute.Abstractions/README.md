[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Custom Attribute Classes

### CodeAttribute & DescriptionAttribute

 _Enum Attribute to add a Code string value to an enum_

 _NOTE: Description attrbute is found in the System.ComponentModel.DescriptionAttribute_ 

```csharp
public enum ApplicationTypes
{
    [Code("new-app")] 
    [Description("New Application")] 
    New = 1000,

    [Code("renew-app")] 
    [Description("Renew Application")] 
    Renew = 2000,
}
```

#### GetCodeEnum Extension

_Extension to return string value from CodeAttribute_
```csharp
ApplicationTypes.New.GetCodeEnum();  //new-app

ApplicationTypes.Renew.GetDescription();  //Renew Application
```

### KeyValueAttribute

```csharp
public enum ApplicationTypes
{
    [KeyValue("Custom", "Another Test 1")]
    New = 1000,

    [KeyValue("Custom", "Value Test 2")]
    [KeyValue("Another", "Another Value")]
    Renew = 2000,
}
```

#### GetKeyValueAttributes Extension

_Extension to return KeyValueAttribute from Enum_
```csharp
ApplicationTypes.New.GetKeyValueAttributes().FirstOrDefault(p => p.Key == "Custom")?.Value;


ApplicationTypes.Renew.GetKeyValueAttributes().Where(p => p.Key == "Custom");
```

### UniqueIdAttribute

```csharp
public enum ApplicationTypes
{
    [UniqueId("1393fff9-3850-4bb2-848b-18973a9f88d0")]
    New = 1000,
}
```

#### GetKeyValueAttributes Extension

_Extension to return Guid value from UniqueId_
```csharp
var uid = OltAttributeExtensions.GetAttributeInstance<UniqueIdAttribute, ApplicationTypes>(ApplicationTypes.New)?.UniqueId;
```

### SortOrderAttribute

```csharp
public enum ApplicationTypes
{
    [SortOrder(10)]
    New = 1000,
}
```

#### OltSortOrderAttributeExtensions Extension

_Extension to return Sort Order or default value_
```csharp
var sortOrder = ApplicationTypes.New.GetSortOrderEnum();
```


### Not Empty Attributes 

| Utility/Item/Object      | Description                            | 
| ------------------------ | -------------------------------------- | 
| OltNotGuidEmptyAttribute | Validation that Guid is not Guid.Empty | 
| OltNotNullAttribute      | Validation that object is not null     | 


```csharp

public class MyPersonModel
{
    [OltNotGuidEmpty]
    public Guid Id { get; set; }

    [StringLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Value { get; set; }

    [OltNotNull]
    public MyOtherModel Other { get; set; }

}

```

