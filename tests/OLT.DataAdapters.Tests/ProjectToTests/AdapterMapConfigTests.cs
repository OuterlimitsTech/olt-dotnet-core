using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;
using OLT.DataAdapters.Tests.ProjectToTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OLT.DataAdapters.Tests.ProjectToTests
{
    public class AdapterMapConfigTests : BaseAdpaterTests
    {

        [Fact]
        public void BeforeMapTests()
        {
            var list = QueryableAdapterObject3.FakerList(300);


            var orderByFunc = new OltBeforeMapOrderBy<QueryableAdapterObject3, QueryableAdapterObject4>(e => e.OrderBy(p => p.First).ThenByDescending(p => p.Last));
            Assert.True(OltAdapterMapConfigs.BeforeMap.Register<QueryableAdapterObject3, QueryableAdapterObject4>(orderByFunc, false));
            Assert.Throws<OltAdapterMapConfigExistsException<QueryableAdapterObject3, QueryableAdapterObject4>>(() => OltAdapterMapConfigs.BeforeMap.Register<QueryableAdapterObject3, QueryableAdapterObject4>(new AdapterObject3ToAdapterObject4BeforeMap(), true));

            var query = OltAdapterMapConfigs.BeforeMap.Apply<QueryableAdapterObject3, QueryableAdapterObject4>(list.AsQueryable());

            query.Should().BeEquivalentTo(list.OrderBy(p => p.First).ThenByDescending(p => p.Last), opt => opt.WithStrictOrdering());

            var mapped = query.Select(s => new QueryableAdapterObject4 { First = s.First, Last = s.Last }).ToList();
            var results = OltAdapterMapConfigs.AfterMap.Apply<QueryableAdapterObject3, QueryableAdapterObject4>(mapped.AsQueryable()).ToList();
            var expected = mapped.OrderBy(p => p.First).ThenByDescending(p => p.Last).ToList();

            results.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void AfterMapTests()
        {
            var list = QueryableAdapterObject3.FakerList(300);

            var orderByAfterMap = new OltAfterMapOrderBy<QueryableAdapterObject3, QueryableAdapterObject5>(e => e.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName));
            Assert.True(OltAdapterMapConfigs.AfterMap.Register<QueryableAdapterObject3, QueryableAdapterObject5>(orderByAfterMap, false));
            Assert.Throws<OltAdapterMapConfigExistsException<QueryableAdapterObject3, QueryableAdapterObject5>>(() => OltAdapterMapConfigs.AfterMap.Register<QueryableAdapterObject3, QueryableAdapterObject5>(new AdapterObject3ToAdapterObject5AfterMap(), true));

            var query = OltAdapterMapConfigs.BeforeMap.Apply<QueryableAdapterObject3, QueryableAdapterObject1>(list.AsQueryable());
            query.Should().BeEquivalentTo(list);

            var mapped = query.Select(s => new QueryableAdapterObject5 { FirstName = s.First, LastName = s.Last }).ToList();
            var results = OltAdapterMapConfigs.AfterMap.Apply<QueryableAdapterObject3, QueryableAdapterObject5>(mapped.AsQueryable()).ToList();

            var expected = mapped.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName).ToList();
            results.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void BeforeMapExtensionTests()
        {
            AdapterObject3ToAdapterObject6QueryableAdapter adapter = null;
            IOltBeforeMap<QueryableAdapterObject3, QueryableAdapterObject6> beforeMap = null;
            Func<IQueryable<QueryableAdapterObject3>, IOrderedQueryable<QueryableAdapterObject3>> func = null;

            Assert.Throws<ArgumentNullException>(nameof(beforeMap), () => OltAdapterExtensions.BeforeMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), beforeMap));
            Assert.Throws<ArgumentNullException>(nameof(func), () => OltAdapterExtensions.WithOrderBy(new AdapterObject3ToAdapterObject6QueryableAdapter(), func));            

            var orderBy = new OltBeforeMapOrderBy<QueryableAdapterObject3, QueryableAdapterObject6>(p => p.OrderBy(t => t.First));

            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.WithOrderBy(adapter, p => p.OrderBy(p => p.Last)));
            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.BeforeMap(adapter, orderBy));

            try
            {
                OltAdapterExtensions.WithOrderBy(new AdapterObject3ToAdapterObject6QueryableAdapter(), p => p.OrderBy(p => p.Last));
                OltAdapterExtensions.BeforeMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), orderBy);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void AfterMapExtensionTests()
        {
            AdapterObject3ToAdapterObject6QueryableAdapter adapter = null;
            IOltAfterMap<QueryableAdapterObject3, QueryableAdapterObject6> afterMap = null;
            Func<IQueryable<QueryableAdapterObject6>, IOrderedQueryable<QueryableAdapterObject6>> func = null;

            Assert.Throws<ArgumentNullException>(nameof(afterMap), () => OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), afterMap));
            Assert.Throws<ArgumentNullException>(nameof(func), () => OltAdapterExtensions.WithOrderBy(new AdapterObject3ToAdapterObject6QueryableAdapter(), func));

            var orderBy = new OltAfterMapOrderBy<QueryableAdapterObject3, QueryableAdapterObject6>(p => p.OrderBy(t => t.FirstName));

            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.WithOrderBy(adapter, p => p.OrderBy(p => p.LastName)));
            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.AfterMap(adapter, orderBy));

            try
            {
                OltAdapterExtensions.WithOrderBy(new AdapterObject3ToAdapterObject6QueryableAdapter(), p => p.OrderBy(p => p.LastName));
                OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), orderBy);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }
}
