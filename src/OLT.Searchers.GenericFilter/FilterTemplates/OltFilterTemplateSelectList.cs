using OLT.Constants;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public class OltFilterTemplateSelectList : OltFilterTemplate<int>, IOltFilterTemplateValueList<int>
    {
        private readonly IOltValueListItem<int> _defaultValue;

        public OltFilterTemplateSelectList(
            string key,
            string label,
            IOltValueListItem<int> defaultValue,
            IEnumerable<IOltValueListItem<int>> valueList,            
            bool hidden = false) : base(key, label, hidden)
        {
            ValueList = valueList.ToList();
            _defaultValue = defaultValue;            
            Value = _defaultValue.Value;           
        }

        
        public override string TemplateName => OltGenericParameterTemplates.SelectList;
        public override bool HasValue => Value > int.MinValue;
        public virtual List<IOltValueListItem<int>> ValueList { get; }
        

        public override bool Parse(IOltGenericParameter parameters)
        {
            var val = parameters?.GetValue(Key, int.MinValue);
            if (val.HasValue && val > int.MinValue)
            {
                Value = val.Value;
                return true;
            }

            Value = _defaultValue.Value;
            return true;

        }

        public override string? Formatted()
        {
            return ValueList.Find(p => p.Value == Value)?.Label ?? ValueList.Find(p => p.Value == _defaultValue.Value)?.Label; 
        }
    }
}
