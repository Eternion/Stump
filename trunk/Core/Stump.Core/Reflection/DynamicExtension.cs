using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Stump.Core.Reflection
{
    public static class DynamicExtension
    {
        /// <summary>
        /// Create a delegate for an empty constructor
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="ctor">Constructor</param>
        /// <returns></returns>
        public static Func<T> CreateDelegate<T>(this ConstructorInfo ctor)
        {
            var dynamicMethod = new DynamicMethod(string.Empty, typeof(T), Type.EmptyTypes, ctor.DeclaringType, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.DeclareLocal(ctor.DeclaringType);
            ilGenerator.Emit(OpCodes.Newobj, ctor);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        /// <summary>
        /// Create a delegate for an action, flemme pour une fonction x]
        /// </summary>
        /// <param name="method"></param>
        /// <param name="delegParams"></param>
        /// <returns></returns>
        public static Delegate CreateDelegate(this MethodInfo method, params Type[] delegParams)
        {
            var methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (delegParams.Length != methodParams.Length)
                throw new Exception("Le nombre d'arguments passé ne correspond pas au nombre d'arguments de la methode");

            var dynamicMethod = new DynamicMethod(string.Empty, null, delegParams);
            var ilGenerator = dynamicMethod.GetILGenerator();

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].IsSubclassOf(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception("Impossible de réaliser un cast vers un object de ce type");
            }
            ilGenerator.Emit(OpCodes.Call, method);

            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(Expression.GetActionType(delegParams));
        }
    }
}
