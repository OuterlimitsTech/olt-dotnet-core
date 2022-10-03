////using Microsoft.Extensions.DependencyInjection;
////using OLT.Core.Rules.Tests.Assets.Rules;
////using System;
////using Xunit;

////namespace OLT.Core.Rules.Tests
////{

////    public class ActionRuleTests : BaseUnitTests
////    {

////        [Fact]
////        public void RusultTests()
////        {
////            var resultValid = new OltRuleResultValid();
////            Assert.True(resultValid.Success);
////            Assert.False(resultValid.Invalid);
////            Assert.Empty(resultValid.Results);

////            var resultInValid = new OltRuleResultInvalid(new OltValidationError(Faker.Lorem.Paragraph()));
////            Assert.False(resultInValid.Success);
////            Assert.True(resultInValid.Invalid);
////            Assert.Empty(resultInValid.Results);

////        }

////        [Fact]
////        public void GetTestRules()
////        {
////            using (var provider = BuildProvider())
////            {
////                var ruleManager = provider.GetService<IOltRuleManager>();
////                var rules = ruleManager.GetRules<ITestRule>();
////                Assert.Equal(6, rules.Count);
////            }
////        }

////        [Fact]
////        public void GetByConcreteClass()
////        {
////            using (var provider = BuildProvider())
////            {
////                var ruleManager = provider.GetService<IOltRuleManager>();
////                var rule = ruleManager.GetRule<TestRuleMulipleInterface>();
////                Assert.True(rule.Execute(new SimpleRequest { ValueRequest = Faker.Name.First() }).Success);
////                Assert.Throws<OltRuleException>(() => rule.Execute(new SimpleRequest()));

////                Assert.True(rule.Execute(new SimpleModelRequest(new RequestModel { Name = Faker.Name.First() })).Success);
////                Assert.Throws<OltRuleException>(() => rule.Execute(new SimpleModelRequest(new RequestModel())));

////                Assert.Single(ruleManager.GetRules<TestRuleMulipleInterface>());

////                Assert.Throws<OltRuleException>(() => ruleManager.GetRule<TestRuleMulipleInterface>().Execute(new SimpleModelRequest(new RequestModel())));
////                Assert.Throws<OltRuleException>(() => ruleManager.GetRule<TestRuleMulipleInterface>().Execute(new SimpleModelRequest(new RequestModel())));
////            }

////        }

////        [Fact]
////        public void NotFound()
////        {
////            using (var provider = BuildProvider())
////            {
////                var ruleManager = provider.GetService<IOltRuleManager>();
////                Assert.Throws<OltRuleNotFoundException>(() => ruleManager.GetRule<INotValidRule>());
////                Assert.Throws<InvalidOperationException>(() => ruleManager.GetRule<NotValidRule>());
////                Assert.Empty(ruleManager.GetRules<INotValidRule>());
////            }            
////        }

////        [Fact]
////        public void ResultValid()
////        {
////            using (var provider = BuildProvider())
////            {
////                var ruleManager = provider.GetService<IOltRuleManager>();
////                var rule = ruleManager.GetRule<TestRuleValid>();
////                Assert.NotNull(rule.Execute(new SimpleRequest { ValueRequest = Faker.Name.First() }) as IOltResultValidation);
////            }
////        }


////    }
////}