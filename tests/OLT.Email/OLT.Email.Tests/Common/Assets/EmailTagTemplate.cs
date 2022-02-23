using System.Collections.Generic;

namespace OLT.Email.Tests.Common.Assets
{
    public class EmailTagTemplate : OltEmailTagTemplate
    {
        public override string TemplateId => nameof(EmailTagTemplate);

        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public override List<OltEmailTag> Tags => new List<OltEmailTag>
        {
            new OltEmailTag(nameof(Value1), Value1),
            new OltEmailTag(nameof(Value2), Value2),
        };

    }
}
