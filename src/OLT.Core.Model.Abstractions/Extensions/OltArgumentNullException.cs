﻿using System.Diagnostics.CodeAnalysis;

#if NETSTANDARD

namespace OLT.Core
{
    internal static class OltArgumentNullException
    {
        public static void ThrowIfNull([NotNull] object? argument, string paramName = null)
        {
            if (argument == null)
            {
                throw new System.ArgumentNullException(paramName);
            }
        }
    }
}

#endif
