namespace OLT.Core
{
    [Obsolete("Being Removed in 9.x")]
    public interface IOltEntityMaintainable : IOltEntity
    {
        bool? MaintAdd { get; set; }
        bool? MaintUpdate { get; set; }
        bool? MaintDelete { get; set; }
    }
}