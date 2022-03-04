using FluentAssertions;
using OLT.Core;
using OLT.EF.Common.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OLT.EF.Common.Tests
{
    public class QueryableExtensionsPagedTests
    {
        private readonly Func<IQueryable<PersonEntity>, IQueryable<PersonEntity>> _defaultOrder = entity => entity.OrderBy(p => p.NameLast).ThenBy(p => p.NameFirst);

        [Fact]
        public void ToPaged()
        {
            var list = PersonEntity.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            var paged = OltQueryableExtensions.ToPaged(queryable, @params);

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
            var list = PersonEntity.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            var paged = OltQueryableExtensions.ToPaged(queryable, @params, _defaultOrder);

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
            var list = PersonEntity.FakerList(153);
            var queryable = list.AsQueryable();

            var @params = new OltPagingParams
            {
                Page = Faker.RandomNumber.Next(1, 4),
                Size = Faker.RandomNumber.Next(10, 20)
            };

            Assert.Throws<ArgumentNullException>(() => OltQueryableExtensions.ToPaged<PersonEntity>(null, null));
            Assert.Throws<ArgumentNullException>(() => OltQueryableExtensions.ToPaged<PersonEntity>(queryable, null));
            Assert.Throws<ArgumentNullException>(() => OltQueryableExtensions.ToPaged<PersonEntity>(null, @params));
        }

        private List<PersonEntity> ExpectedPage(List<PersonEntity> list, OltPagingParams pagingParams)
        {
            return list.Skip((pagingParams.Page - 1) * pagingParams.Size).Take(pagingParams.Size).ToList();
        }
    }
}
