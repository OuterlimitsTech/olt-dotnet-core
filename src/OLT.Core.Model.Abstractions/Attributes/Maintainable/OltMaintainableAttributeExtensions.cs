namespace OLT.Core
{
    [Obsolete("Being Removed in 9.x")]
    public static class OltMaintainableAttributeExtensions
    {

        /// <summary>
        /// Returns <see cref="MaintainableAttribute"/> attribute
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        [Obsolete("Being Removed in 9.x")]
        public static MaintainableAttribute GetMaintainable(this Enum @enum)
        {
            return GetAttributeInstance(@enum);
        }

        /// <summary>
        /// Sets the <see cref="IOltEntityMaintainable"/> fields using <see cref="MaintainableAttribute"/> attribute        
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="entity"></param>
        /// <param name="enum"></param>
        [Obsolete("Being Removed in 9.x")]
        public static void SetMaintainable<TEntity, TEnum>(this TEntity entity, TEnum @enum)
            where TEntity : class, IOltEntityMaintainable
            where TEnum : System.Enum
        {
            var value = GetMaintainable(@enum);            
            entity.MaintAdd = MaintainableAttribute.ToBool(value.Create);            
            entity.MaintUpdate = MaintainableAttribute.ToBool(value.Update);
            entity.MaintDelete = MaintainableAttribute.ToBool(value.Delete);
        }

        /// <summary>
        /// Returns first instance of <typeparamref name="T"/> attribute on <seealso cref="Enum"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>First instance of <see cref="Attribute"/> to search for or <see langword="null"/></returns>
        /// <exception cref="InvalidOperationException">Sequence contains more than one element</exception>
        private static MaintainableAttribute GetAttributeInstance(Enum item)            
        {
            if (item == null) return new MaintainableAttribute();

            var type = item.GetType();
            var attribute = type.GetField(item.ToString())?.GetCustomAttributes(typeof(MaintainableAttribute), false).Cast<MaintainableAttribute>().SingleOrDefault();
            return attribute ?? new MaintainableAttribute();
        }

    }
}
