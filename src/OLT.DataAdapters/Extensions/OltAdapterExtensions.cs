namespace OLT.Core
{
    public static class OltAdapterExtensions
    {
        public static string BuildAdapterName<TObj1, TObj2>()
        {
            return $"{typeof(TObj1).FullName}->{typeof(TObj2).FullName}";
        }

        public static string BuildAfterMapName<TObj1, TObj2>()
        {
            return $"{BuildAdapterName<TObj1, TObj2>()}_AfterMap";
        }

        public static string BuildBeforeMapName<TObj1, TObj2>()
        {
            return $"{BuildAdapterName<TObj1, TObj2>()}_BeforeMap";
        }
    }
}
