using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace Peppy.Extensions
{
    /// <summary>
    /// Enum of extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the enum of description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, (object)value);
            if (name == null) return (string)null;
            var field = type.GetField(name);
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false) is DescriptionAttribute customAttribute)
                return customAttribute.Description;
            return null;
        }

        /// <summary>
        /// Get the enum of name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, (object)value);
            if (name == null)
                return (string)null;
            var field = type.GetField(name);
            return field?.Name;
        }
    }
}