using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class QueryableExtensionsTests
    {
        public class PersonEntity
        {
            public string NameFirst { get; set; }
            public string NameLast { get; set; }
        }

        [Fact]
        public void OrderByPropertyName()
        {
            var list = new List<PersonEntity>()
            {
                new PersonEntity
                {
                    NameFirst = "Todd",
                    NameLast = "Gabriel"
                },
                new PersonEntity
                {
                    NameFirst = "Charlie",
                    NameLast = "Apple"
                },
                new PersonEntity
                {
                    NameFirst = "Jamie",
                    NameLast = "Beatriz"
                },

            };

            var compareToAsc = list.OrderBy(p => p.NameLast).ToList();
            var actualAsc = list.AsQueryable().OrderByPropertyName(nameof(PersonEntity.NameLast), true).ToList();

            actualAsc.Should().BeEquivalentTo(compareToAsc, options => options.WithStrictOrdering());

            var compareToDesc = list.OrderByDescending(p => p.NameLast).ToList();
            var actualDesc = list.AsQueryable().OrderByPropertyName(nameof(PersonEntity.NameLast), false).ToList();
            actualDesc.Should().BeEquivalentTo(compareToDesc, options => options.WithStrictOrdering());
        }
    }
}
