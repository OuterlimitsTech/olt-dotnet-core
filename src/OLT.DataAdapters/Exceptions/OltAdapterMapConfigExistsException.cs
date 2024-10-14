using System;

namespace OLT.Core
{
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltAdapterMapConfigExistsException : Exception
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {

        public OltAdapterMapConfigExistsException(string configMapName, string source, string destination) :
            base($"{configMapName} already exists for {source} -> {destination}")
        {

        }

    }

#pragma warning disable S3925 // "ISerializable" should be implemented correctly
    public class OltAdapterMapConfigExistsException<TSource, TDestination> : OltAdapterMapConfigExistsException
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
    {

        public OltAdapterMapConfigExistsException(IOltAdapterMapConfig<TSource, TDestination> configMap) :  base(OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>(), typeof(TSource).FullName ?? "Unknown",  typeof(TDestination).FullName ?? "Unknown")
        {

        }

    }
}