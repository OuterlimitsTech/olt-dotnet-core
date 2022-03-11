using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using Xunit;

namespace OLT.DataAdapters.Tests
{
    public abstract class BaseAdapterTests
    {
        protected abstract ServiceProvider BuildProvider();

        //[Fact]
        //public void CanMapTests()
        //{
        //    using (var provider = BuildProvider())
        //    {
        //        var adapterResolver = provider.GetService<IOltAdapterResolver>();
        //        Assert.True(adapterResolver.CanMap<AdapterObject1, AdapterObject2>());
        //        Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject1>());
        //        Assert.True(adapterResolver.CanMap<AdapterObject2, AdapterObject3>());
        //        Assert.True(adapterResolver.CanMap<AdapterObject3, AdapterObject2>());

        //        Assert.False(adapterResolver.CanMap<AdapterObject1, AdapterObject3>());
        //        Assert.False(adapterResolver.CanMap<AdapterObject3, AdapterObject1>());
        //    }
        //}

    }
}