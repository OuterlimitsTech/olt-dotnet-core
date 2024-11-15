[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Custom Attribute Classes

### CodeAttribute

 _Enum Attribute to add a Code string value to an enum_

 _NOTE: Description attrbute is found in the System.ComponentModel.DescriptionAttribute_ 

```csharp
public enum ApplicationTypes
{
    [Code("New")] 
    [Description("New")] 
    New = 1000,

    [Code("Renew")] 
    [Description("Renew")] 
    Renew = 2000,
}
```

#### GetCodeEnum Extension

_Extension to return string value from CodeAttribute_
```csharp
ApplicationTypes.New.GetCodeEnum()
```

#### Not Empty Attributes 

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

