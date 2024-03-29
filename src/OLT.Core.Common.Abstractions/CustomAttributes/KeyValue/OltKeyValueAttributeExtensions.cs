﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OLT.Core
{
    public static class OltKeyValueAttributeExtensions
    {
        /// <summary>
        /// Returns all instances of <see cref="KeyValueAttribute"/> attribute on a enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns all instances of <see cref="KeyValueAttribute"/></returns>
        public static List<KeyValueAttribute> GetKeyValueAttributes(this Enum value)
        {
            var attr = value?.GetType().GetField(value.ToString());
            if (attr == null) return new List<KeyValueAttribute>();

            return attr
                .GetCustomAttributes(typeof(KeyValueAttribute), false)
                .ToList()
                .Cast<KeyValueAttribute>()
                .ToList();
        }
    }
}
