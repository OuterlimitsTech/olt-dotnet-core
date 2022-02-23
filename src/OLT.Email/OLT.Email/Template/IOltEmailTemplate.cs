using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltEmailTemplate
    {
        string TemplateId { get; }
        object GetTemplateData();
    }

    public interface IOltEmailTemplate<TEmailAddress> : IOltEmailTemplate
        where TEmailAddress : class, IOltEmailAddress
    {
        List<TEmailAddress> To { get; }
    }


}