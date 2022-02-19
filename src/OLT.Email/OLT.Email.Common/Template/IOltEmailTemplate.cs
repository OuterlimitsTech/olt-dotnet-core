using System.Collections.Generic;

namespace OLT.Email
{
    public interface IOltEmailTemplate
    {
        string TemplateName { get; }
        object GetTemplateData();
    }

    public interface IOltEmailTemplate<out TEmailAddress> : IOltEmailTemplate
        where TEmailAddress : class, IOltEmailAddress
    {
        IEnumerable<TEmailAddress> To { get; }
    }


}