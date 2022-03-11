//using FluentAssertions;
//using OLT.Core;
//using OLT.EF.Common.Tests.Assets.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace OLT.EF.Common.Tests
//{
//    public class QueryableExtensionsOrderByTests
//    {
//        private readonly Func<IQueryable<PersonEntity>, IQueryable<PersonEntity>> _defaultOrder = entity => entity.OrderBy(p => p.NameFirst);

//        [Fact]
//        public void OrderByPropertyName()
//        {
//            var list = PersonEntity.FakerList(25);
//            var queryable = list.AsQueryable();

//            OltWhereExtensions.OrderByPropertyName(queryable, nameof(PersonEntity.NameLast), true)
//                .Should()
//                .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

//            OltWhereExtensions.OrderByPropertyName(queryable, nameof(PersonEntity.NameLast), false)
//                .Should()
//                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
//        }

//        [Fact]
//        public void OrderBySortParams()
//        {
//            var list = PersonEntity.FakerList(25);
//            var queryable = list.AsQueryable();

//            var sort = new OltSortParams
//            {
//                PropertyName = nameof(PersonEntity.NameLast),
//                IsAscending = true,
//            };

            
//            OltWhereExtensions.OrderBy(queryable, sort)
//                .Should()
//                .BeEquivalentTo(list.OrderBy(p => p.NameLast), options => options.WithStrictOrdering());

//            sort = new OltSortParams
//            {
//                PropertyName = nameof(PersonEntity.NameLast),
//                IsAscending = false,
//            };

//            OltWhereExtensions.OrderBy(queryable, sort)
//                .Should()
//                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());
//        }

//        [Fact]
//        public void OrderBySortParamsWithDefault()
//        {

//            var list = PersonEntity.FakerList(25);
//            var queryable = list.AsQueryable();

//            var sort = new OltSortParams
//            {
//                PropertyName = nameof(PersonEntity.NameLast),
//                IsAscending = false,
//            };

//            OltWhereExtensions.OrderBy(queryable, sort, _defaultOrder)
//                .Should()
//                .BeEquivalentTo(list.OrderByDescending(p => p.NameLast), options => options.WithStrictOrdering());


//            OltWhereExtensions.OrderBy(queryable, null, _defaultOrder)
//                .Should()
//                .BeEquivalentTo(list.OrderBy(p => p.NameFirst), options => options.WithStrictOrdering());


//        }

//        [Fact]
//        public void OrderByExceptions()
//        {
            
//            var list = PersonEntity.FakerList(25);
//            var queryable = list.AsQueryable();
//            var sort = new OltSortParams
//            {
//                PropertyName = nameof(PersonEntity.NameLast),
//                IsAscending = false,
//            };

//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy<PersonEntity>(null, null));
//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy<PersonEntity>(null, sort));

//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy<PersonEntity>(null, null, null));
//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy(null, null, _defaultOrder));
//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy(null, sort, _defaultOrder));
//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy(queryable, sort, null));
//            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.OrderBy(queryable, null, null));
//        }
//    }
//}
