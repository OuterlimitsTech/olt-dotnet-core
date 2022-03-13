﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;
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
            const string value = "S";
            var list = AdapterObject3.FakerList(300);

            OltAdapterMapConfigs.BeforeMap.Register<AdapterObject3, AdapterObject4>(new AdapterObject3ToAdapterObject4BeforeMap(value));
            OltAdapterMapConfigs.BeforeMap.Register<AdapterObject3, AdapterObject4>(new OltBeforeMapOrderBy<AdapterObject3, AdapterObject4>(e => e.OrderBy(p => p.First).ThenByDescending(p => p.Last)));

            var query = OltAdapterMapConfigs.ApplyBeforeMaps<AdapterObject3, AdapterObject4>(list.AsQueryable());

            query.Should().BeEquivalentTo(list.OrderBy(p => p.First).ThenByDescending(p => p.Last).Where(p => p.Last.Contains(value)), opt => opt.WithStrictOrdering());

            var mapped = query.Select(s => new AdapterObject4 { First = s.First, Last = s.Last }).ToList();
            var results = OltAdapterMapConfigs.ApplyAfterMaps<AdapterObject3, AdapterObject4>(mapped.AsQueryable()).ToList();
            var expected = mapped.OrderBy(p => p.First).ThenByDescending(p => p.Last).Where(p => p.Last.Contains(value));

            results.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void AfterMapTests()
        {
            const string value = "A";
            var list = AdapterObject3.FakerList(300);

            OltAdapterMapConfigs.AfterMap.Register<AdapterObject3, AdapterObject5>(new AdapterObject3ToAdapterObject5AfterMap(value));
            OltAdapterMapConfigs.AfterMap.Register<AdapterObject3, AdapterObject5>(new OltAfterMapOrderBy<AdapterObject3, AdapterObject5>(e => e.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName)));

            var query = OltAdapterMapConfigs.ApplyBeforeMaps<AdapterObject3, AdapterObject1>(list.AsQueryable());
            query.Should().BeEquivalentTo(list);

            var mapped = query.Select(s => new AdapterObject5 { FirstName = s.First, LastName = s.Last }).ToList();
            var results = OltAdapterMapConfigs.ApplyAfterMaps<AdapterObject3, AdapterObject5>(mapped.AsQueryable()).ToList();

            var expected = mapped.OrderBy(p => p.LastName).ThenByDescending(p => p.FirstName).Where(p => p.FirstName.Contains(value));
            results.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void BeforeMapExtensionTests()
        {
            AdapterObject3ToAdapterObject6QueryableAdapter adapter = null;
            IOltBeforeMap<AdapterObject3, AdapterObject6> beforeMap = null;
            Func<IQueryable<AdapterObject3>, IQueryable<AdapterObject3>> func = null;

            Assert.Throws<ArgumentNullException>(nameof(beforeMap), () => OltAdapterExtensions.BeforeMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), beforeMap));
            Assert.Throws<ArgumentNullException>(nameof(func), () => OltAdapterExtensions.BeforeMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), func));

            var orderBy = new OltBeforeMapOrderBy<AdapterObject3, AdapterObject6>(p => p.OrderBy(t => t.First));

            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.BeforeMap(adapter, p => p.OrderBy(p => p.Last)));
            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.BeforeMap(adapter, orderBy));

            try
            {
                OltAdapterExtensions.BeforeMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), p => p.OrderBy(p => p.Last));
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
            IOltAfterMap<AdapterObject3, AdapterObject6> afterMap = null;
            Func<IQueryable<AdapterObject6>, IQueryable<AdapterObject6>> func = null;

            Assert.Throws<ArgumentNullException>(nameof(afterMap), () => OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), afterMap));
            Assert.Throws<ArgumentNullException>(nameof(func), () => OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), func));

            var orderBy = new OltAfterMapOrderBy<AdapterObject3, AdapterObject6>(p => p.OrderBy(t => t.FirstName));

            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.AfterMap(adapter, p => p.OrderBy(p => p.LastName)));
            Assert.Throws<ArgumentNullException>(nameof(adapter), () => OltAdapterExtensions.AfterMap(adapter, orderBy));

            try
            {
                OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), p => p.OrderBy(p => p.LastName));
                OltAdapterExtensions.AfterMap(new AdapterObject3ToAdapterObject6QueryableAdapter(), orderBy);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }
}
