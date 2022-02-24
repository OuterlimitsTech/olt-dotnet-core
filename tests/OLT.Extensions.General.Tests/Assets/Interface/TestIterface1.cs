using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLT.Extensions.General.Tests.Assets.Interface
{
    public class TestIterface1 : ITestInterface
    {

    }

    public class TestIterface2 : ITestInterface
    {

    }

    public class TestIterface3 : ITestInterface
    {

    }

    public class TestIterface4 : ITestInterface<TestIterface4>
    {
        public TestIterface4 Value { get; set; }
    }

    public class TestIterface4<T> : ITestInterface<T>
        where T : class
    {
        public T Value { get; set; }
    }

    public class TestIterface5<T> : ITestInterface
        where T : class
    {
        public T Value { get; set; }
    }
}
