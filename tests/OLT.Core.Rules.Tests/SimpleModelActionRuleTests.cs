using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Rules;
using System.Linq;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class SimpleModelActionRuleTests : BaseUnitTests
    {
        public static TheoryData<ITestRuleSimpleModelRequest, SimpleModelRequest, IOltRuleResult, bool> SimpleModelData
        {
            get
            {
                var results = new TheoryData<ITestRuleSimpleModelRequest, SimpleModelRequest, IOltRuleResult, bool>();
                results.Add(new TestRuleBadRequest(), new SimpleModelRequest(new RequestModel()), new OltRuleResultInvalid(new OltValidationError(TestRuleBadRequest.Message)), false);
                results.Add(new TestRuleFailure(), new SimpleModelRequest(new RequestModel()), null, true);
                results.Add(new TestRuleMulipleInterface(), new SimpleModelRequest(new RequestModel()), null, true);
                results.Add(new TestRuleMulipleInterface(), new SimpleModelRequest(new RequestModel { Name = Faker.Name.First() }), OltRuleResultHelper.Success, false);
                return results;
            }
        }

        [Theory]
        [MemberData(nameof(SimpleModelData))]
        public void RuleResultTests(ITestRuleSimpleModelRequest testRule, SimpleModelRequest request, IOltRuleResult expected, bool throwsException)
        {
            using (var provider = BuildProvider())
            {
                var ruleManager = provider.GetService<IOltRuleManager>();
                var rules = ruleManager.GetRules<ITestRule>();
                var rule = rules.First(p => p.RuleName == testRule.RuleName);
                Assert.NotNull(rule);
                var typedRule = rule as ITestRuleSimpleModelRequest;
                Assert.NotNull(typedRule);

                if (throwsException)
                {
                    Assert.Throws<OltRuleException>(() => typedRule.Execute(request));
                }
                else
                {
                    var result = typedRule.Execute(request);
                    result.Should().BeEquivalentTo(expected);
                }

            }
        }
    }
}