namespace OLT.Core
{
    public class OltSortParams : IOltSortParams 
    {
        public string? PropertyName { get; set; }
        public bool IsAscending { get; set; }
    }
}