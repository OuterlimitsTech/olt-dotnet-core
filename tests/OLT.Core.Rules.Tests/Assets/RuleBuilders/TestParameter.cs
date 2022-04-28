using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Core.Rules.Tests.Assets.RuleBuilders
{
    public class TestParameter
    {
        public string Name { get; set; } = Faker.Name.FullName();
    }
}
