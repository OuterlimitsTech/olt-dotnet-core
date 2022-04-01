using OLT.Constants;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public class OltFilterTemplateSelectListNullable : OltFilterTemplate<int?>, IOltFilterTemplateValueList<int>
    {

        public OltFilterTemplateSelectListNullable(
            string key,
            string label,
            IEnumerable<IOltValueListItem<int>> valueList,
            bool hidden = false) : base(key, label, hidden)
        {
            ValueList = valueList.ToList();
        }

        public override string TemplateName  => OltGenericParameterTemplates.SelectList;
        public override bool HasValue => Value != null && Value > 0;
        public virtual List<IOltValueListItem<int>> ValueList { get; }

        public override bool Parse(IOltGenericParameter parameters)
        {
            var val = parameters.GetValue(Key, int.MinValue);
            Value = val > int.MinValue ? val : new int?();
            return HasValue;
        }

        public override string ToString()
        {
            return Value?.ToString();
        }
    }
}
