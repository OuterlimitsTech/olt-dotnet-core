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
            Assert.Equal(3, assembliesToScan.GetAllImplements<ITestInterface>().Count());
            Assert.Equal(3, assembliesToScan.ToArray().GetAllImplements<ITestInterface>().Count());

            Assert.Empty(assembliesToScan.GetAllImplements<ITestEmptyInterface>());
            Assert.Empty(assembliesToScan.ToArray().GetAllImplements<ITestEmptyInterface>());
        }
    }
}