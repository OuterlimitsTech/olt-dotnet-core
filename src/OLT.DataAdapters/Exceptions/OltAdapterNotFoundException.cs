using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OLT.Core
{


    public class OltAdapterNotFoundException : OltException
    {
        public OltAdapterNotFoundException(string adapterName) : base($"Adapter Not Found {adapterName}")
        {

        }

    }

    public class OltAdapterNotFoundException<TSource, TDestination> : OltException
    {
        public OltAdapterNotFoundException() : base($"Adapter Not Found {OltAdapterExtensions.BuildAdapterName<TSource, TDestination>()}")
        {

        }


    }
}