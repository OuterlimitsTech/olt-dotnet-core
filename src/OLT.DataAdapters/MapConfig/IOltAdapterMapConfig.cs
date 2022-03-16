namespace OLT.Core
{
    public interface IOltAdapterMapConfig
    {
        //string Name { get; }
    }

    public interface IOltAdapterMapConfig<TSource, TDestination> : IOltAdapterMapConfig
    {
        
    }



}