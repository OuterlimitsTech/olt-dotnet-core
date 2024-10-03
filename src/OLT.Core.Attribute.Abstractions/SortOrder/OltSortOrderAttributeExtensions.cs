namespace OLT.Core
{
    public static class OltSortOrderAttributeExtensions
    {
        /// <summary>
        /// Returns SortOrder value from <see cref="SortOrderAttribute"/> attribute
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultSortOrder"></param>
        /// <returns><see cref="short"/> SortOrder from <see cref="SortOrderAttribute"/> or <see langword="null"/></returns>        
        public static short? GetSortOrderEnum(this Enum value, short defaultSortOrder = 9999)
        {
            var attr = value?.GetType().GetField(value.ToString());
            if (attr == null) return defaultSortOrder;

            var attribute = attr
                .GetCustomAttributes(typeof(SortOrderAttribute), false)
                .Cast<SortOrderAttribute>()
                .SingleOrDefault();
            return attribute?.SortOrder ?? defaultSortOrder;
        }
    }
}