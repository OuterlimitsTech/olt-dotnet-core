using FluentAssertions;
using Newtonsoft.Json.Linq;
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

            //Check passing in default for a existing value
            short intDefault = (short)Faker.RandomNumber.Next();
            ruleIntValue = GetValue<int>("intValue", intDefault);
            Assert.Equal(intValue, ruleIntValue);

            //Check passing in default for a non-existing value
            short shortExpected = (short)Faker.RandomNumber.Next(short.MaxValue);
            var nonValue = GetValue<short>("shortValue", shortExpected);
            Assert.Equal(shortExpected, nonValue);


            //Check mising value exception
            Assert.Throws<OltRuleMissingValueException<string>>(() => GetValue<string>("strValue1"));

            return Task.CompletedTask;
        }


        public TValue GetValueTest<TValue>(string key) where TValue : IConvertible
        {
            return GetValue<TValue>(key);
        }

        public TValue GetValueTest<TValue>(string key, TValue defaultValue) where TValue : IConvertible
        {
            return GetValue<TValue>(key, defaultValue);
        }

    }
}
