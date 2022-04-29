namespace OLT.Core
{
    public class OltDependentRule
    {
        public OltDependentRule(OltDependentRuleRunTypes type, IOltActionRule rule)
        {
            Rule = rule;
            Type = type;
        }

        public OltDependentRuleRunTypes Type { get; set; }
        public IOltActionRule Rule { get; }
    }
}
