using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    public static class OltMaintainableAttributeExtensions
    {

        /// <summary>
        /// Returns <see cref="MaintainableAttribute"/> attribute
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static MaintainableAttribute GetMaintainable(this Enum @enum)
        {
            return OltAttributeExtensions.GetAttributeInstance<MaintainableAttribute>(@enum) ?? new MaintainableAttribute();
        }

        /// <summary>
        /// Sets the <see cref="IOltEntityMaintainable"/> fields using <see cref="MaintainableAttribute"/> attribute        
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="entity"></param>
        /// <param name="enum"></param>
        public static void SetMaintainable<TEntity, TEnum>(this TEntity entity, TEnum @enum)
            where TEntity : class, IOltEntityMaintainable
            where TEnum : System.Enum
        {
            GetMaintainable(@enum).SetEntityValues(entity);
        }
    }
}
