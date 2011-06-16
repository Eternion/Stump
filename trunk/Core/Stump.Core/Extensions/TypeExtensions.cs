
using System;

namespace Stump.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfGeneric(this Type type, Type genericType)
        {
            var t = type.BaseType;

            while (t != null && !t.IsValueType)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == genericType)
                    return true;

                t = t.BaseType;
            }

            return false;
        }
    }
}