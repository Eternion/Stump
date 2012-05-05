using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Stump.Core.Collections;
using Stump.Core.Extensions;

namespace Stump.Core.Reflection
{
    public static class ReflectionExtensions
    {
        public static Type GetActionType(this MethodInfo method)
        {
            return Expression.GetActionType(method.GetParameters().Select(entry => entry.ParameterType).ToArray());
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.FindInterfaces(FilterByName, interfaceType).Length > 0;
        }

        private static bool FilterByName(Type typeObj, Object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }

        public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider type)
            where T : Attribute
        {
            var attribs = type.GetCustomAttributes(typeof(T), false) as T[];
            return attribs;
        }

        public static T GetCustomAttribute<T>(this ICustomAttributeProvider type)
            where T : Attribute
        {
            return type.GetCustomAttributes<T>().GetOrDefault(0);
        }

        public static bool IsDerivedFromGenericType(this Type type, Type genericType)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type == null)
            {
                return false;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            return IsDerivedFromGenericType(type.BaseType, genericType);
        }
    }
}