using System;
using System.Collections.Generic;
using System.Reflection;

namespace OLT.Core
{

#if NET6_0_OR_GREATER

    public class OltAssemblyFullNameComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly? x, Assembly? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Assembly obj)
        {
            return obj.FullName?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
        }
    }

#else

   public class OltAssemblyFullNameComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly? x, Assembly? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return string.Equals(x.FullName, y.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Assembly obj)
        {
            return obj.FullName?.GetHashCode() ?? 0;
        }
    }

#endif



}
