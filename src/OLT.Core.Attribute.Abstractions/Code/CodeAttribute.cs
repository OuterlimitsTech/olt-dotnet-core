namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CodeAttribute : Attribute
    {
        public string Code { get; private set; }

        public CodeAttribute(string code)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(code);
            this.Code = code;
        }
    }
}