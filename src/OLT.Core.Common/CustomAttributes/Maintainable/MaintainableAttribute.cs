using System;

namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MaintainableAttribute : Attribute
    {

        public OltEntityMaintainable Create { get; set; } = OltEntityMaintainable.NotSet;
        public OltEntityMaintainable Update { get; set; } = OltEntityMaintainable.NotSet;
        public OltEntityMaintainable Delete { get; set; } = OltEntityMaintainable.NotSet;

        public bool? ToBool(OltEntityMaintainable value)
        {
            return value == OltEntityMaintainable.NotSet ? new bool?() : value == OltEntityMaintainable.Yes;
        }
    }
}