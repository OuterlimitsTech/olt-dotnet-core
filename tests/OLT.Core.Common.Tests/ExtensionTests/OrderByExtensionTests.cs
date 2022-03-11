using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Linq;
using Xunit;


namespace OLT.Core.Common.Tests.ExtensionTests
{  

    public class OrderByExtensionsTests
    {
        private readonly Func<IQueryable<TestPersonNameModel>, IQueryable<TestPersonNameModel>> _defaultOrder = entity => entity.OrderBy(p => p.NameFirst);

        [Fact]
        public void OrderByPropertyName()
        {
            var list = TestPersonNameModel.FakerList(25);
            var queryable = list.AsQueryable();

            OltOrderByExtensions.OrderByPropertyName(queryable, nameof(TestPersonNameModel.NameLast), true)
                .Should()
                .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

            OltOrderByExtensions.OrderByPropertyName(queryable, nameof(TestPersonNameModel.NameLast), false)
                .Should()
                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
        }

        [Fact]
        public void OrderBySortParams()
        {
            var list = TestPersonNameModel.FakerList(25);
            var queryable = list.AsQueryable();

            var sort = new OltSortParams
            {
                PropertyName = nameof(TestPersonNameModel.NameLast),
                IsAscending = true,
            };


            OltOrderByExtensions.OrderBy(queryable, sort)
                .Should()
                .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

            sort = new OltSortParams
            {
                PropertyName = nameof(TestPersonNameModel.NameLast),
                IsAscending = false,
            };

            OltOrderByExtensions.OrderBy(queryable, sort)
                .Should()
                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
        }

        [Fact]
        public void OrderBySortParamsWithDefault()
        {

            var list = TestPersonNameModel.FakerList(25);
            var queryable = list.AsQueryable();

            var sort = new OltSortParams
            {
                PropertyName = nameof(TestPersonNameModel.NameLast),
                IsAscending = false,
            };

            OltOrderByExtensions.OrderBy(queryable, sort, _defaultOrder)
                .Should()
                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());


            OltOrderByExtensions.OrderBy(queryable, null, _defaultOrder)
                .Should()
                .BeEquivalentTo(list.OrderBy(p => p.NameFirst), options => options.WithStrictOrdering());


        }

        [Fact]
        public void OrderByExceptions()
        {

            var list = TestPersonNameModel.FakerList(25);
            var queryable = list.AsQueryable();
            var sort = new OltSortParams
            {
                PropertyName = nameof(TestPersonNameModel.NameLast),
                IsAscending = false,
            };

            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, null));
            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, sort));

            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy<TestPersonNameModel>(null, null, null));
            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(null, null, _defaultOrder));
            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(null, sort, _defaultOrder));
            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(queryable, sort, null));
            Assert.Throws<ArgumentNullException>(() => OltOrderByExtensions.OrderBy(queryable, null, null));
        }
    }
}
