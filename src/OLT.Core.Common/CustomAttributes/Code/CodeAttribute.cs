using System;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CodeAttribute : Attribute
    {
        public string Code { get; private set; }
        public short DefaultSort { get; set; } = 9999;

        public CodeAttribute(string code)
        {
            this.Code = code;
        }
        public CodeAttribute(string code, short defaultSort)
        {
            this.Code = code;
            this.DefaultSort = defaultSort;
        }
    }
}