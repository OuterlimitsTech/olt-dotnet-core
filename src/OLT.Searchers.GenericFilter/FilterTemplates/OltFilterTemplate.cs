namespace OLT.Core
{

    public abstract class OltFilterTemplate<TValueType> : OltGenericParameterParser<TValueType>, IOltFilterTemplate
    {
        protected OltFilterTemplate(string key, string label, bool hidden = false) : base(key)
        {
            Label = label;
            Hidden = hidden;
        }

        public virtual string Label { get; }
        public virtual bool Hidden { get; }
        public abstract string TemplateName { get; }

        public new abstract string ToString();
    }
}
