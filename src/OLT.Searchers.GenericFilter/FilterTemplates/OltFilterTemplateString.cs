using OLT.Constants;

namespace OLT.Core
{
    public class OltFilterTemplateString : OltFilterTemplate<string?>
    {
        public OltFilterTemplateString(string key, string label, bool hidden = false) : base(key, label, hidden)
        {
        }

        public override string TemplateName => OltGenericParameterTemplates.String;
        public override bool HasValue => !string.IsNullOrWhiteSpace(Value);


        public override bool Parse(IOltGenericParameter parameters)
        {
            if (parameters == null)
            {
                Value = null;
                return false;
            }

            var val = parameters.GetValue(Key, string.Empty);
            Value = string.IsNullOrWhiteSpace(val) ? null : val;

            return HasValue;
        }

        public override string? Formatted()
        {
            return Value;
        }
    }
}
