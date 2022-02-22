using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltEmailTagTemplate : IOltEmailTemplate
    {
        List<OltEmailTag> Tags { get; }
    }

    public interface IOltEmailTagTemplate<TEmailAddress> : IOltEmailTagTemplate
        where TEmailAddress : class, IOltEmailAddress
    {
        List<TEmailAddress> To { get; }
    }
}