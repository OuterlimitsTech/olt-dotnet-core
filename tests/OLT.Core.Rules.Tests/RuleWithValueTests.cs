using OLT.Core.Rules.Tests.Assets.Context;
using OLT.Core.Rules.Tests.Assets.RuleBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OLT.Core.Rules.Tests
{
    public class RuleWithValueTests : BaseUnitTests
    {
        [Fact]
        public async Task ExecuteTests()
        {
            using (var provider = BuildProvider())
            {
                var rule = new Test4Rule();
                rule.WithValue("intValue", rule.intValue)
                    .WithValue("strValue", rule.strValue);

                try
                {
                    using (var tran = new MockTran())
                    {
                        await rule.ExecuteAsync(tran);
                    }
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }
            }

            using (var provider = BuildProvider())
            {
                var rule = new Test4Rule();
                try
                {
                    using (var tran = new MockTran())
                    {
                        await rule.ExecuteAsync(tran);
                    }
                    Assert.True(false);
                }
                catch
                {
                    Assert.True(true);
                }
            }

        }


        [Fact]
        public void GetValueTests()
        {
            const string bogusKey = "bogusValue";

            var intValue = Faker.RandomNumber.Next();
            var strValue1 = Faker.Name.First();
            var strValue2 = Faker.Name.Last();
            var rule = new Test4Rule();
            rule.WithValue("intValue", intValue)
                .WithValue("strValue", strValue1);

            Assert.Equal(intValue, rule.GetValueTest<int>("intValue"));
            Assert.Equal(strValue1, rule.GetValueTest<string>("strValue"));
            Assert.Throws<ArgumentException>(() => rule.WithValue("strValue", 1234));

            Assert.Equal(strValue1, rule.GetValueTest<string>("strValue", strValue2));            
            Assert.NotEqual(strValue1, rule.GetValueTest<string>(bogusKey, strValue2));
            Assert.Equal(strValue2, rule.GetValueTest<string>(bogusKey, strValue2));

            var exception = Assert.Throws<OLT.Core.OltRuleMissingValueException<string>>(() => rule.GetValueTest<string>(bogusKey));
            Assert.Equal($"{rule.RuleName} requires {bogusKey} value of type {typeof(string).FullName}", exception.Message);
        }

    }
}