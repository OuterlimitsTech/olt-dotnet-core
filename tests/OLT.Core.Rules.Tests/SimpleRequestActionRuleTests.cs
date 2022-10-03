////using FluentAssertions;
////using Microsoft.Extensions.DependencyInjection;
////using OLT.Constants;
////using OLT.Core.Rules.Tests.Assets.Rules;
////using System.Linq;
////using Xunit;

////namespace OLT.Core.Rules.Tests
////{
////    public class SimpleRequestActionRuleTests : BaseUnitTests
////    {
////        public static TheoryData<ITestRuleSimpleRequest, SimpleRequest, IOltRuleResult, bool> SimpleModelData
////        {
////            get
////            {
////                var results = new TheoryData<ITestRuleSimpleRequest, SimpleRequest, IOltRuleResult, bool>();
////                results.Add(new TestRule(), new SimpleRequest(), OltRuleResultHelper.Success, false);
////                results.Add(new TestRuleInValid(), new SimpleRequest(), new OltRuleResultInvalid(new OltValidationError(OltRuleDefaults.InvalidMessage)), false);
////                results.Add(new TestRuleMulipleInterface(), new SimpleRequest(), null, true);
////                results.Add(new TestRuleMulipleInterface(), new SimpleRequest { ValueRequest = Faker.Name.First() }, OltRuleResultHelper.Success, false);
////                results.Add(new TestRuleValid(), new SimpleRequest(), OltRuleResultHelper.Success, false);
////                return results;
////            }
////        }

////        [Theory]
////        [MemberData(nameof(SimpleModelData))]
////        public void RuleResultTests(ITestRuleSimpleRequest testRule, SimpleRequest request, IOltRuleResult expected, bool throwsException)
////        {
////            using (var provider = BuildProvider())
////            {
////                var ruleManager = provider.GetService<IOltRuleManager>();
////                var rules = ruleManager.GetRules<ITestRule>();
////                var rule = rules.First(p => p.RuleName == testRule.RuleName);
////                Assert.NotNull(rule);
////                var typedRule = rule as ITestRuleSimpleRequest;
////                Assert.NotNull(typedRule);

////                if (throwsException)
////                {
////                    Assert.Throws<OltRuleException>(() => typedRule.Execute(request));
////                }
////                else
////                {
////                    var result = typedRule.Execute(request);
////                    result.Should().BeEquivalentTo(expected);
////                }

////            }
////        }
////    }
////}