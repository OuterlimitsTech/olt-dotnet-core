using System;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class UniqueIdAttribute : Attribute
    {
        public Guid UniqueId { get; private set; }

        private UniqueIdAttribute() { }
        public UniqueIdAttribute(string uniqueId)
        {
            // Just parse it - things should blow up if it doesn't work.
            this.UniqueId = Guid.Parse(uniqueId);
        }      
    }
}