namespace OLT.Core
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltAdapterMapConfigExists : OltException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {

        public OltAdapterMapConfigExists(string configMapName, string source, string destination) :
            base($"{configMapName} already exists for {source} -> {destination}")
        {

        }

    }

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltAdapterMapConfigExists<TSource, TDestination> : OltAdapterMapConfigExists
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {

        public OltAdapterMapConfigExists(IOltAdapterMapConfig<TSource, TDestination> configMap) :  base(OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>(), typeof(TSource).FullName,  typeof(TDestination).FullName)
        {

        }

    }
}