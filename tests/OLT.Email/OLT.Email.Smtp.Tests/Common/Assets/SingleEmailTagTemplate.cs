using System.Collections.Generic;

namespace OLT.Email.Smtp.Tests.Common.Assets
{
    public class SingleEmailTagTemplate : OltSingleEmailTagTemplate
    {
        public override string TemplateName => nameof(SingleEmailTagTemplate);

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override List<OltEmailTag> Tags => new List<OltEmailTag>
        {
            new OltEmailTag(nameof(FirstName), FirstName),
            new OltEmailTag(nameof(LastName), LastName),
        };

    }
}
