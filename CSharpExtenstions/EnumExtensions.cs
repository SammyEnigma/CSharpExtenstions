using CSharpExtenstions.Helpers;
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

        /// <summary>
        /// Retrieves the multiple description on the enum
        /// </summary>
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

        /// <summary>
        /// Converts Enum to List with Key being Enum Property Name and Value being the Enum Value of that property
        /// </summary>
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
            }

            return list;
        }

        /// <summary>
        /// Gets the Attribute associated to the Enum
        /// </summary>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        /// <summary>
        /// Gets the Description of the Attribute
        /// </summary>
        public static string ToName(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static bool IsSet<T>(this T value, T flags) where T : struct
        {
            Type type = typeof(T);

            if (!type.IsEnum)
                throw Error.Argument("T", "The type parameter T must be an enum type.");

            Type numberType = Enum.GetUnderlyingType(type);

            if (numberType.Equals(typeof(int)))
            {
                return BoxUnbox<int>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(sbyte)))
            {
                return BoxUnbox<sbyte>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(byte)))
            {
                return BoxUnbox<byte>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(short)))
            {
                return BoxUnbox<short>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(ushort)))
            {
                return BoxUnbox<ushort>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(uint)))
            {
                return BoxUnbox<uint>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(long)))
            {
                return BoxUnbox<long>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(ulong)))
            {
                return BoxUnbox<ulong>(value, flags, (a, b) => (a & b) == b);
            }
            else if (numberType.Equals(typeof(char)))
            {
                return BoxUnbox<char>(value, flags, (a, b) => (a & b) == b);
            }
            else
            {
                throw new ArgumentException("Unknown enum underlying type " +
                    numberType.Name + ".");
            }
        }

        private static bool BoxUnbox<T>(object value, object flags, Func<T, T, bool> op)
        {
            return op((T)value, (T)flags);
        }
    }
}