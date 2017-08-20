using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WalkReader.Model
{
    public static class Helper
    {

        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, Boolean nameInstead = true)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }

            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }

        public static object GetValue(this object m1, string prop)
        {
            var fld = m1.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public);
            if (fld == null)
            {
                return null;
            }
            else
            {
                return fld.GetValue(m1, null);
            }
        }

        public static T GetEnum<T>(string value, T defaultValue)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T should be enum");
            }

            foreach (var obj in Enum.GetValues(typeof(T)))
            {
                var s = obj;
                if (Helper.GetDescription((Enum)s) == value)
                {
                    return (T)s;
                }
            }

            return defaultValue;
        }
    }
}
