using System.Collections.Generic;

namespace OLT.Email.SendGrid
{
    public interface IOltEmailTemplate
    {
        OltEmailRecipients Recipients { get; }
    }

    public interface IOltEmailTagTemplate : IOltEmailTemplateId, IOltEmailTemplate
    {        
        List<OltEmailTag> Tags { get; }
    }

    public interface IOltEmailTemplateId
    {
        string TemplateId { get; }
        object GetTemplateData();
    }

    public interface IOltEmailJsonTemplate<out TModel> : IOltEmailTemplateId, IOltEmailTemplate
        where TModel : class
    {
        TModel TemplateData { get; }
    }

}