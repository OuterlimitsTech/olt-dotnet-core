using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Linq;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public class GeneralTests : BaseAdpaterTests
    {
        [Fact]
        public void DisposableTests()
        {
            using (var adapter1 = new AdapterObject4PagedMap())
            {
                Assert.False(adapter1.IsDisposed());
            }

            var adapter = new AdapterObject4PagedMap();
            Assert.NotNull(adapter);
            Assert.False(adapter.IsDisposed());
            adapter.Dispose();
            Assert.True(adapter.IsDisposed());
        }

    }
}