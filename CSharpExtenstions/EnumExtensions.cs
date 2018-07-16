using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CSharpExtenstions
{
    public static class EnumExtenstions
    {
        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static IEnumerable<string> GetDescriptions(Enum value)
        {
            var descs = new List<string>();
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var field = type.GetField(name);
            var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
            foreach (DescriptionAttribute fd in fds)
            {
                descs.Add(fd.Description);
            }
            return descs;
        }

        public static Dictionary<string, string> ToList(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            Dictionary<string, string> list = new Dictionary<string, string>();
            Array enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                list.Add(value.ToString(), GetDescription(value));
                //list.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }

            return list;
        }
    }
}
