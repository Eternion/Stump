using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        /// <summary>
        /// Search for a method by name and parameter types.  Unlike GetMethod(), does 'loose' matching on generic
        /// parameter types, and searches base interfaces.
        /// </summary>
        /// <exception cref="AmbiguousMatchException"/>
        public static MethodInfo GetMethodExt(this Type thisType, string name, int genericArgumentsCount, params Type[] parameterTypes)
        {
            return GetMethodExt(thisType, name, genericArgumentsCount, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, parameterTypes);
        }

        /// <summary>
        /// Search for a method by name, parameter types, and binding flags.  Unlike GetMethod(), does 'loose' matching on generic
        /// parameter types, and searches base interfaces.
        /// </summary>
        /// <exception cref="AmbiguousMatchException"/>
        public static MethodInfo GetMethodExt(this Type thisType, string name, int genericArgumentsCount, BindingFlags bindingFlags, params Type[] parameterTypes)
        {
            MethodInfo matchingMethod = null;

            // Check all methods with the specified name, including in base classes
            GetMethodExt(ref matchingMethod, thisType, name, genericArgumentsCount, bindingFlags, parameterTypes);

            // If we're searching an interface, we have to manually search base interfaces
            if (matchingMethod == null && thisType.IsInterface)
            {
                foreach (Type interfaceType in thisType.GetInterfaces())
                    GetMethodExt(ref matchingMethod, interfaceType, name, genericArgumentsCount, bindingFlags, parameterTypes);
            }

            return matchingMethod;
        }

        private static void GetMethodExt(ref MethodInfo matchingMethod, Type type, string name, int genericArgumentsCount, BindingFlags bindingFlags, params Type[] parameterTypes)
        {
            // Check all methods with the specified name, including in base classes
            foreach (MethodInfo methodInfo in type.GetMember(name, MemberTypes.Method, bindingFlags))
            {
                if (methodInfo.GetGenericArguments().Length != genericArgumentsCount)
                    continue;
                
                // Check that the parameter counts and types match, with 'loose' matching on generic parameters
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length == parameterTypes.Length)
                {
                    int i = 0;
                    for (; i < parameterInfos.Length; ++i)
                    {
                        if (!parameterInfos[i].ParameterType.IsSimilarType(parameterTypes[i]))
                            break;
                    }
                    if (i == parameterInfos.Length)
                    {
                        if (matchingMethod == null)
                            matchingMethod = methodInfo;
                        else
                            throw new AmbiguousMatchException("More than one matching method found!");
                    }
                }
            }
        }

        /// <summary>
        /// Special type used to match any generic parameter type in GetMethodExt().
        /// </summary>
        public class T
        {
        }

        /// <summary>
        /// Determines if the two types are either identical, or are both generic parameters or generic types
        /// with generic parameters in the same locations (generic parameters match any other generic paramter,
        /// but NOT concrete types).
        /// </summary>
        private static bool IsSimilarType(this Type thisType, Type type)
        {
            // Ignore any 'ref' types
            if (thisType.IsByRef)
                thisType = thisType.GetElementType();
            if (type.IsByRef)
                type = type.GetElementType();

            // Handle array types
            if (thisType.IsArray && type.IsArray)
                return thisType.GetElementType().IsSimilarType(type.GetElementType());

            // If the types are identical, or they're both generic parameters or the special 'T' type, treat as a match
            if (thisType == type || ( ( thisType.IsGenericParameter || thisType == typeof(T) ) && ( type.IsGenericParameter || type == typeof(T) ) ))
                return true;

            // Handle any generic arguments
            if (thisType.IsGenericType && type.IsGenericType)
            {
                Type[] thisArguments = thisType.GetGenericArguments();
                Type[] arguments = type.GetGenericArguments();
                if (thisArguments.Length == arguments.Length)
                {
                    for (int i = 0; i < thisArguments.Length; ++i)
                    {
                        if (!thisArguments[i].IsSimilarType(arguments[i]))
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }
    }
}