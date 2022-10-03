namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public interface ITestRuleBogusService : IOltCoreService  //Not registered
    {

    }

    public interface ITestRuleService : IOltCoreService
    {
        bool TestMethod();
    }
}
