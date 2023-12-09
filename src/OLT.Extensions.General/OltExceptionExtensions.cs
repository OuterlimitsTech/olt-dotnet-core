using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class OltExceptionExtensions
    {
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }
    }


}

#if NETSTANDARD
namespace System
{    

    public static class OltArgumentNullException
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
