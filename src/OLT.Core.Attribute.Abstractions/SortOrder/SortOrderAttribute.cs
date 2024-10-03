namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SortOrderAttribute : Attribute
    {
        public static short SortOrderDefault = 9999;

        public SortOrderAttribute()
        {
        }

        public SortOrderAttribute(short sortOrder)
        {
            this.SortOrder = sortOrder;
        }

        public short SortOrder { get; set; } = SortOrderDefault;

    }
}