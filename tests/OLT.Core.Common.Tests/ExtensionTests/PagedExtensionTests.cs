using FluentAssertions;
using OLT.Core.Common.Tests.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.Core.Common.Tests.ExtensionTests
{

    public class PagedExtensionTests
    {
        private readonly Func<IQueryable<TestPersonNameModel>, IQueryable<TestPersonNameModel>> _defaultOrder = entity => entity.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst);

        [Fact]
        public void ToPaged()
        {
            var list = TestPersonNameModel.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            var paged = OltPagedExtensions.ToPaged(queryable, @params);

            Assert.Equal(@params.Page, paged.Page);
            Assert.Equal(@params.Size, paged.Size);
            Assert.Equal(list.Count, paged.Count);
            Assert.Equal(@params.Size, paged.Data.Count());

            var expected = ExpectedPage(list, @params);
            paged.Data.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void ToPagedOrdered()
        {
            var list = TestPersonNameModel.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            var paged = OltPagedExtensions.ToPaged(queryable, @params, _defaultOrder);

            Assert.Equal(@params.Page, paged.Page);
            Assert.Equal(@params.Size, paged.Size);
            Assert.Equal(list.Count, paged.Count);
            Assert.Equal(@params.Size, paged.Data.Count());

            var expected = ExpectedPage(_defaultOrder(list.AsQueryable()).ToList(), @params);
            paged.Data.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());

        }

        [Fact]
        public void ToPagedExceptions()
        {
            var list = TestPersonNameModel.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(null, null));
            Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(queryable, null));
            Assert.Throws<ArgumentNullException>(() => OltPagedExtensions.ToPaged<TestPersonNameModel>(null, @params));
        }

        private List<TestPersonNameModel> ExpectedPage(List<TestPersonNameModel> list, OltPagingParams pagingParams)
        {
            return list.Skip((pagingParams.Page - 1) * pagingParams.Size).Take(pagingParams.Size).ToList();
        }
    }
}
