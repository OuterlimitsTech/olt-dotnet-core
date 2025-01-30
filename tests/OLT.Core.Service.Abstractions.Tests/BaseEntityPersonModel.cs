namespace OLT.Core.Service.Abstractions.Tests;

public abstract class BaseEntityPersonModel : IOltEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
