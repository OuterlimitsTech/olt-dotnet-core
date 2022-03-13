using FluentAssertions;
using OLT.Core;
using OLT.EF.Common.Tests.Assets;
using OLT.EF.Common.Tests.Assets.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.EF.Common.Tests
{
    public class QueryableExtensionsWhereTests
    {

        [Fact]
        public void WhereExtensionTests()
        {
            var firstName = nameof(WhereExtensionTests);
            var searcher = new PersonFirstNameStartsWithSearcher(firstName);

            var expected = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25), true));
            expected.SetFirstNameStartsWith(firstName);

            var list = TestHelper.BuildTestList(expected);
            var queryable = list.AsQueryable();


            var results = OltWhereExtensions.Where(queryable, searcher).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher = new PersonFirstNameStartsWithSearcher(firstName, true);

            results = OltWhereExtensions.Where(queryable, searcher).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher = new PersonFirstNameStartsWithSearcher(firstName, false);
            Assert.False(searcher.IncludeDeleted);
            results = OltWhereExtensions.Where(queryable, searcher).ToList();
            results.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhereExtensionArrayTests()
        {
            var firstName = $"First_{nameof(WhereExtensionArrayTests)}";
            var lastName = $"Last_{nameof(WhereExtensionArrayTests)}";
            var searcher1 = new PersonLastNameStartsWithSearcher(lastName);
            var searcher2 = new PersonFirstNameStartsWithSearcher(firstName);

            var expected = EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25));
            expected.AddRange(EntityPersonModel.FakerList(Faker.RandomNumber.Next(10, 25), true));
            expected.SetFirstNameStartsWith(firstName);
            expected.SetLastNameStartsWith(lastName);

            var list = TestHelper.BuildTestList(expected);
            var queryable = list.AsQueryable();

            Assert.NotEmpty(list.Where(p => p.DeletedOn == null));
            Assert.NotEmpty(list.Where(p => p.DeletedOn != null));

            var results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);

            searcher2 = new PersonFirstNameStartsWithSearcher(firstName, true);
            results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);


            searcher2 = new PersonFirstNameStartsWithSearcher(firstName, false);
            results = queryable.Where(searcher1, searcher2).ToList();
            results.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void WhereExceptions()
        {
            var list = PersonEntity.FakerList(20);            
            var queryable = list.AsQueryable();
            IOltSearcher<PersonEntity>[] searchers = null;

            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where<PersonEntity>(null, searchers));
            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where<PersonEntity>(queryable, searchers));


            IOltSearcher<PersonEntity> searcher = null;
            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where<PersonEntity>(null, searcher));
            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where<PersonEntity>(queryable, searcher));            
        }
    }
}
