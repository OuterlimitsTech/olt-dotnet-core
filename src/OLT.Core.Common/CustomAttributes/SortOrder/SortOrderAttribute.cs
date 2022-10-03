using OLT.Constants;
using System;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SortOrderAttribute : Attribute
    {
        public SortOrderAttribute()
        {
            SortOrder = OltCommonDefaults.SortOrder;
        }

        public SortOrderAttribute(short sortOrder)
        {
            this.SortOrder = sortOrder;
        }

        public short SortOrder { get; set; }

    }
}