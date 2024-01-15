using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public class OltFilterTemplateMultiSelectList : OltFilterTemplate<List<int>>, IOltFilterTemplateValueList<int>
    {
        public OltFilterTemplateMultiSelectList(
            string key,
            string label,            
            IEnumerable<IOltValueListItem<int>> valueList,
            bool hidden = false) : base(key, label, hidden)
        {
            ValueList = valueList.ToList();
        }

        public override string TemplateName => OltGenericParameterTemplates.MultiSelectList;
        public override bool HasValue => Value != null && Value.Any();
        public virtual List<IOltValueListItem<int>> ValueList { get; }

        public override bool Parse(IOltGenericParameter parameters)
        {
            if (parameters == null)
            {
                return false;
            }

            Value = parameters.GetValue(Key, string.Empty)
                .Split(',')
                .Select(value => OltStringExtensions.ToInt(value, -1))
                .Where(p => p > 0)
                .ToList();

            return HasValue;
        }

        public override string? Formatted()
        {
            return Value == null ? null : string.Join(",", ValueList.Where(item => Value.Contains(item.Value)).Select(s => s.Label));
        }
    }
}
