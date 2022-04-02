using OLT.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public class OltFilterTemplateDateRange : OltFilterTemplate<OltDateRange>, IOltFilterTemplateValueList<OltDateRange>
    {
        private readonly OltDateRange _defaultValue;

        public OltFilterTemplateDateRange(
            string key,
            string label,
            OltDateRange defaultValue,
            IEnumerable<IOltValueListItem<OltDateRange>> valueList, 
            bool hidden = false) : base(key, label, hidden)
        {
            ValueList = valueList.ToList();
            Value = _defaultValue = defaultValue;
        }

        public override string TemplateName => OltGenericParameterTemplates.DateRange;
        public override bool HasValue => Value != null;
        public virtual List<IOltValueListItem<OltDateRange>> ValueList { get; }


        public override bool Parse(IOltGenericParameter parameters)
        {
            if (parameters != null)
            {
                var start = parameters.GetValue($"{Key}-start", string.Empty);
                var end = parameters.GetValue($"{Key}-end", string.Empty);

                if (start.IsDate() && end.IsDate())
                {
                    this.Value = new OltDateRange
                    {
                        Start = new DateTimeOffset(start.ToDate().GetValueOrDefault(_defaultValue.Start.LocalDateTime)),
                        End = new DateTimeOffset(end.ToDate().GetValueOrDefault(_defaultValue.End.LocalDateTime))
                    };
                    return true;
                }
            }

            Value = _defaultValue;
            return true;
        }

        public override string Formatted()
        {
            return $"{Value?.Start.LocalDateTime:M/d/yyyy} to {Value?.End.LocalDateTime:M/d/yyyy}";
        }
    }
}
