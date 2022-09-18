using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OLT.Core;
using OLT.Extensions.General.Tests.Assets.Interface;
using Xunit;

namespace OLT.Extensions.General.Tests
{
    public class SystemReflectionTest
    {

        [Fact]
        public void GetAllImplements()
        {
            var baseAssemblies = new List<Assembly>
            {
                Assembly.GetEntryAssembly(), 
                Assembly.GetExecutingAssembly()
            };
            var assembliesToScan = baseAssemblies.GetAllReferencedAssemblies();
            Assert.Equal(4, assembliesToScan.GetAllImplements<ITestInterface>().Count());
            Assert.Equal(4, assembliesToScan.ToArray().GetAllImplements<ITestInterface>().Count());
            Assert.Equal(4, this.GetType().Assembly.GetAllImplements<ITestInterface>().Count());

            Assert.Empty(assembliesToScan.GetAllImplements<ITestEmptyInterface>());
            Assert.Empty(assembliesToScan.ToArray().GetAllImplements<ITestEmptyInterface>());
            Assert.Empty(this.GetType().Assembly.GetAllImplements<ITestEmptyInterface>());
        }


        [Fact]
        public void ImplementTests()
        {
            Assert.True(new TestIterface1().GetType().Implements<ITestInterface>());
            Assert.False(new TestIterface1().GetType().Implements<ITestEmptyInterface>());
            Assert.Throws<InvalidOperationException>(() => new TestIterface1().GetType().Implements<TestIterface2>());


            Assert.True(new TestIterface4().GetType().Implements(typeof(ITestInterface<>)));
            Assert.False(new TestIterface1().GetType().Implements(typeof(ITestInterface<>)));

            Assert.Throws<InvalidOperationException>(() => new TestIterface4().GetType().Implements(typeof(TestIterface5<>)));
            
        }
    }
}