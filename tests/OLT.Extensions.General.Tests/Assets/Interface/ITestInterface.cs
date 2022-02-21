namespace OLT.Extensions.General.Tests.Assets.Interface
{
    public interface ITestInterface
    {
    }

    public interface ITestInterface<T> : ITestInterface 
        where T : class
    {
        public T Value { get; set; }
    }
}
