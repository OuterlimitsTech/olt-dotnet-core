using OLT.Constants;
using System;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CodeAttribute : Attribute
    {
        public string Code { get; private set; }

        [Obsolete("Move to SortOrderAttribute")]
        public short DefaultSort { get; set; } = OltCommonDefaults.SortOrder;

        public CodeAttribute(string code)
        {
            this.Code = code;
        }

        [Obsolete("Move to SortOrderAttribute")]
        public CodeAttribute(string code, short defaultSort)
        {
            this.Code = code;
            this.DefaultSort = defaultSort;
        }
    }
}