namespace OLT.Core
{
    public interface IOltValueListItem<T>
    {
        T? Value { get; set; }
        string? Label { get; set; }
    }
}
