using System;

namespace OLT.Core
{
    public class OltAdapterNotFoundException : Exception
    {
        public OltAdapterNotFoundException(string adapterName) : base($"Adapter Not Found {adapterName}")
        {

        }
    }

    public class OltAdapterNotFoundException<TSource, TDestination> : Exception
    {
        public OltAdapterNotFoundException() : base($"Adapter Not Found {OltAdapterExtensions.BuildAdapterName<TSource, TDestination>()}")
        {

        }
    }


}