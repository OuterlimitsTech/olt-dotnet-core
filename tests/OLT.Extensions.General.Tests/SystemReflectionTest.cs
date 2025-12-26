using OLT.Extensions.General.Tests.Assets.Interface;
using OLT.Utility.AssemblyScanner;
using System.Reflection;

namespace OLT.Extensions.General.Tests
{
    public class SystemReflectionTest
    {
        [Fact]
        public void GetAllImplements()
        {
            var assembliesToScan = new OltAssemblyScanBuilder().IncludeAssembly(Assembly.GetEntryAssembly(), Assembly.GetExecutingAssembly()) .Build();

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