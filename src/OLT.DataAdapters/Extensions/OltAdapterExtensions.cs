namespace OLT.Core
{
    public static class OltAdapterExtensions
    {
        public static string BuildAdapterName<TObj1, TObj2>()
        {
            return $"{typeof(TObj1).FullName}->{typeof(TObj2).FullName}";
        }        
    }
}
