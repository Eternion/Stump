using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Stump.Core.Reflection
{
    public static class DynamicExtension
    {
        public static T CreateDelegate<T>(this ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToList();

            var lamba = Expression.Lambda<T>(Expression.New(ctor, parameters), parameters);

            return lamba.Compile();
        }

        /// <summary>
        /// Create a delegate for an action
        /// </summary>
        /// <param name="method"></param>
        /// <param name="delegParams"></param>
        /// <returns></returns>
        public static Delegate CreateDelegate(this MethodInfo method, params Type[] delegParams)
        {
            var methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (delegParams.Length != methodParams.Length)
                throw new Exception("Method parameters count != delegParams.Length");

            var dynamicMethod = new DynamicMethod(string.Empty, null, delegParams);
            var ilGenerator = dynamicMethod.GetILGenerator();

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].IsSubclassOf(delegParams[i]) || methodParams[i].HasInterface(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception(string.Format("Cannot cast {0} to {1}", methodParams[i].Name, delegParams[i].Name));
            }
            ilGenerator.Emit(OpCodes.Call, method);

            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(Expression.GetActionType(delegParams));
        }
    }
}
