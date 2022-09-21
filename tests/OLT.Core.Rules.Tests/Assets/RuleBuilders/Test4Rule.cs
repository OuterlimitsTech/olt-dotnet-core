using FluentAssertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class Test4Rule : OltActionRule<Test4Rule>
    {

        public int intValue { get; set; } = Faker.RandomNumber.Next();
        public string strValue { get; set; } = Faker.Lorem.Words(10).Last();

        protected override Task RunRuleAsync()
        {
            var ruleIntValue = GetValue<int>("intValue");
            var ruleStrValue = GetValue<string>("strValue");

            Assert.Equal(intValue, ruleIntValue);
            Assert.Equal(strValue, ruleStrValue);

            short expected = (short)Faker.RandomNumber.Next(short.MaxValue);

            var nonValue = GetValue<short>("shortValue", expected);
            Assert.Equal(expected, nonValue);

            Assert.Throws<OltRuleMissingValueException<string>>(() => GetValue<string>("strValue1"));

            return Task.CompletedTask;
        }
    }
}
