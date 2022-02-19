using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace OLT.Email
{
    public abstract class OltSingleEmailTagTemplate<TEmailAddress> : IOltSingleEmailTagTemplate<TEmailAddress>
        where TEmailAddress : class, IOltEmailAddress
    {
        public abstract string TemplateName { get; }
        public virtual TEmailAddress To { get; set; }
        public abstract IEnumerable<OltEmailTag> Tags { get; }        

        public object GetTemplateData()
        {
            return OltEmailTag.ToDictionary(Tags.ToList());
        }
    }

    public abstract class OltSingleEmailTagTemplate : OltSingleEmailTagTemplate<OltEmailAddress>
    {
        
    }
}