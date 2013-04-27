using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Stump.Core.Reflection
{
    public class ObjectDumper
    {
        private const int DepthDisplayLimit = 5;

        private int m_level;
        private readonly int m_indentSize;
        private readonly StringBuilder m_stringBuilder;

        public ObjectDumper(int indentSize)
        {
            m_indentSize = indentSize;
            m_stringBuilder = new StringBuilder();
        }

        public Func<MemberInfo, bool> MemberPredicate
        {
            get;
            set;
        }

        public static string Dump(object element)
        {
            return Dump(element, 2);
        }

        public static string Dump(object element, int indentSize)
        {
            var instance = new ObjectDumper(indentSize);
            return instance.DumpElement(element);
        }

        public string DumpElement(object element)
        {
            if (m_level > DepthDisplayLimit)
                return "... (limit reached)";

            if (element == null || element is ValueType || element is string)
            {
                Write(FormatValue(element));
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    Write("{{{0}}}", objectType.FullName);
                    m_level++;
                }

                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    var count = 0;
                    foreach (object item in enumerableElement)
                    {
                        count++;

                        if (item is IEnumerable && !( item is string ))
                        {
                            m_level++;
                            DumpElement(item);
                            m_level--;
                        }
                        else
                        {
                            DumpElement(item);
                        }
                    }

                    if (count == 0)
                        Write("-Empty-");
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                    foreach (var memberInfo in members)
                    {
                        if (memberInfo is EventInfo)
                            continue;

                        var fieldInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;

                        if (fieldInfo == null && propertyInfo == null)
                            continue;

                        if (MemberPredicate != null && !MemberPredicate(memberInfo))
                            continue;

                        var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;

                        if (propertyInfo != null && propertyInfo.GetIndexParameters().Length > 0)
                            continue;

                        object value = fieldInfo != null
                                           ? fieldInfo.GetValue(element)
                                           : propertyInfo.GetValue(element, null);

                        //events
                        if (value is MulticastDelegate)
                            continue;

                        if (type.IsValueType || type == typeof(string))
                        {
                            Write("{0}: {1}", memberInfo.Name, FormatValue(value));
                        }
                        else
                        {
                            Write("{0}: {1}", memberInfo.Name, typeof(IEnumerable).IsAssignableFrom(type) ? "..." : "{ }");
                            m_level++;
                            DumpElement(value);
                            m_level--;
                        }
                    }
                }

                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    m_level--;
                }
            }

            return m_stringBuilder.ToString();
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(' ', m_level * m_indentSize);

            if (args != null)
                value = string.Format(value, args);

            m_stringBuilder.AppendLine(space + value);
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ( "null" );

            if (o is DateTime)
                return ( ( (DateTime)o ).ToShortDateString() );

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is ValueType)
                return ( o.ToString() );

            if (o is IEnumerable)
                return ( "..." );

            return ( "{ }" );
        }
    }
}