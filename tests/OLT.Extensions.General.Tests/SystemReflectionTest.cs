using FluentAssertions;
using OLT.Core;
using OLT.Extensions.General.Tests.Assets.Interface;
using System.Reflection;

namespace OLT.Extensions.General.Tests
{
    public class SystemReflectionTest
    {

        [Fact]
        public void GetReferencedAssemblies()
        {
            var self = this.GetType().Assembly;
            var list = new List<Assembly>
            {
                self,
                self,  //Listed twice to confirm we get a unique list back
                typeof(OltSystemReflectionExtensions).Assembly
            };

            var assemblies = self.GetAllReferencedAssemblies().ToList();
            Assert.True(self.GetAllReferencedAssemblies().Count() > 100);
            Assert.True(list.ToArray().GetAllReferencedAssemblies().Count() > 100);
            Assert.True(list.GetAllReferencedAssemblies().Count() > 100);
            Assert.Equal(assemblies.Count, list.GetAllReferencedAssemblies().Count());
        }

        [Fact]
        public void GetReferencedWithFilterAssemblies()
        {
            var filter = new Core.OltAssemblyFilter("OLT.Extensions.General");

            var self = this.GetType().Assembly;
            var list = new List<Assembly>
            {
                self,
            };

            Assert.True(filter.Filters.Count == 1);

            Assert.Single(list.GetAllReferencedAssemblies(filter));
            Assert.Single(list.ToArray().GetAllReferencedAssemblies(filter));
            Assert.Single(list.GetAllReferencedAssemblies(filter));

        }

        [Fact]
        public void GetReferencedWithWildcardFilterAssemblies()
        {
            var filter = new Core.OltAssemblyFilter(" ", string.Empty, null, "OLT.*");

            var self = this.GetType().Assembly;
            var list = new List<Assembly>
            {
                self,
            };

            Assert.Equal(4, filter.Filters.Count);
            Assert.Empty(filter.ExcludeFilters);
            var debugTest = list.GetAllReferencedAssemblies(filter).ToList();
            Assert.Equal(2, list.GetAllReferencedAssemblies(filter).Count());
            Assert.Equal(2, list.ToArray().GetAllReferencedAssemblies(filter).Count());            
            Assert.Equal(2, list.GetAllReferencedAssemblies(filter).Count());

        }

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

        [Fact]
        public void GetReferencedWithExcludeFilterAssemblies()
        {
            var filter = new Core.OltAssemblyFilter().WithDefaultDIExclusionFilters();

            var self = this.GetType().Assembly;
            var list = new List<Assembly>
            {
                self,
            };

            Assert.Empty(filter.Filters);
            Assert.Equal(2, filter.ExcludeFilters.Count);
            var filtered = list.GetAllReferencedAssemblies(filter).ToList();

            filtered.Should().NotContain(p => p.FullName.StartsWith("Microsoft"));
            filtered.Should().NotContain(p => p.FullName.StartsWith("System"));

            //By including and excluding the same filter should result in zero results
            filtered = list.GetAllReferencedAssemblies(new OltAssemblyFilter("Microsoft.*", "System.*").WithDefaultDIExclusionFilters()).ToList();
            Assert.Empty(filtered);
        }

        
    }
}