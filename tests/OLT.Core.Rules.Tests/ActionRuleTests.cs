using Microsoft.Extensions.DependencyInjection;
using OLT.Core.Rules.Tests.Assets.Rules;
using System;
using Xunit;

namespace OLT.Core.Rules.Tests
{

    public class ActionRuleTests : BaseUnitTests
    {

        [Fact]
        public void GetRules()
        {
            using (var provider = BuildProvider())
            {
                var ruleManager = provider.GetService<IOltRuleManager>();
                var rules = ruleManager.GetRules<ITestRule>();
                Assert.Equal(3, rules.Count);
            }
        }



        [Fact]
        public void GetByInterface()
        {
            using (var provider = BuildProvider())
            {
                var ruleManager = provider.GetService<IOltRuleManager>();
                var rule = ruleManager.GetRule<ITestRuleSimpleRequest>();
                Assert.True(rule.Execute(new SimpleRequest { ValueRequest = Faker.Name.First() }).Success);
                Assert.Throws<OltRuleException>(() => rule.Execute(new SimpleRequest()));

                Assert.Equal(3, ruleManager.GetRules<ITestRule>().Count);
            }
        }

        [Fact]
        public void GetByConcreteClass()
        {
            using (var provider = BuildProvider())
            {
                var ruleManager = provider.GetService<IOltRuleManager>();
                var rule = ruleManager.GetRule<TestRuleMulipleInterface>();
                Assert.True(rule.Execute(new SimpleRequest { ValueRequest = Faker.Name.First() }).Success);
                Assert.Throws<OltRuleException>(() => rule.Execute(new SimpleRequest()));

                Assert.True(rule.Execute(new SimpleModelRequest(new RequestModel { Name = Faker.Name.First() })).Success);
                Assert.Throws<OltRuleException>(() => rule.Execute(new SimpleModelRequest(new RequestModel())));

                Assert.Single(ruleManager.GetRules<TestRuleMulipleInterface>());
            }

        }

        [Fact]
        public void NotFound()
        {
            using (var provider = BuildProvider())
            {
                var ruleManager = provider.GetService<IOltRuleManager>();
                Assert.Throws<OltRuleNotFoundException>(() => ruleManager.GetRule<INotValidRule>());
                Assert.Throws<InvalidOperationException>(() => ruleManager.GetRule<NotValidRule>());
                Assert.Empty(ruleManager.GetRules<INotValidRule>());
            }            
        }

        //[Fact]
        //public void Failure()
        //{
        //    var result = UnitTestHelper.AddPerson(_personService, UnitTestHelper.CreateTestAutoMapperModel());
        //    var rule = ruleManager.GetRule<DoSomethingRuleFailure>();
        //    Assert.Throws<OltRuleException>(() => rule.Execute(new DoSomethingPersonRuleRequest(result)));
        //}

        //[Fact]
        //public void ValidationInvalid()
        //{
        //    var rule = ruleManager.GetRule<DoSomethingRuleIntValue>();
        //    Assert.True(rule.Validate(new DoSomethingRuleIntRequest(_context, 5)).Invalid);
        //}

        //[Fact]
        //public void ValidationInvalidMessages()
        //{
        //    var rule = ruleManager.GetRule<DoSomethingRuleIntValue>();
        //    var result = rule.Validate(new DoSomethingRuleIntRequest(_context, 5));
        //    Assert.True(result.Results.Any());
        //}
    }
}