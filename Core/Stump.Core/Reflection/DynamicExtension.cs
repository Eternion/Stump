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

        public static Delegate CreateDelegate(this ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToList();

            var lamba = Expression.Lambda(Expression.New(ctor, parameters), parameters);
            
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

            var dynamicMethod = new DynamicMethod(string.Empty, null, new[] { typeof(object) }.Concat(delegParams).ToArray(), true);
            var ilGenerator = dynamicMethod.GetILGenerator();

            if (!method.IsStatic)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(method.DeclaringType.IsClass ? OpCodes.Castclass : OpCodes.Unbox, method.DeclaringType);
            }

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].IsSubclassOf(delegParams[i]) || methodParams[i].HasInterface(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception(string.Format("Cannot cast {0} to {1}", methodParams[i].Name, delegParams[i].Name));
            }

            ilGenerator.Emit(OpCodes.Call, method);

            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(Expression.GetActionType(new[] { typeof(object) }.Concat(delegParams).ToArray()));

        }

        public static Delegate CreateFuncDelegate(this MethodInfo method, Type returnType, params Type[] delegParams)
        {
            var methodParams = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (delegParams.Length != methodParams.Length)
                throw new Exception("Method parameters count != delegParams.Length");

            var dynamicMethod = new DynamicMethod(string.Empty, returnType, new[] { typeof(object) }.Concat(delegParams).ToArray(), true);
            var ilGenerator = dynamicMethod.GetILGenerator();

            if (!method.IsStatic)
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(method.DeclaringType.IsClass ? OpCodes.Castclass : OpCodes.Unbox, method.DeclaringType);
            }

            for (var i = 0; i < delegParams.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                if (delegParams[i] != methodParams[i])
                    if (methodParams[i].IsSubclassOf(delegParams[i]) || methodParams[i].HasInterface(delegParams[i]))
                        ilGenerator.Emit(methodParams[i].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[i]);
                    else
                        throw new Exception(string.Format("Cannot cast {0} to {1}", delegParams[i].Name, methodParams[i].Name));
            }

            ilGenerator.Emit(OpCodes.Call, method);

            if (returnType != null && returnType != method.ReturnType)
                if (method.ReturnType.IsSubclassOf(returnType) || method.ReturnType.HasInterface(returnType))
                {
                    if (method.ReturnType.IsClass && returnType.IsClass)
                        ilGenerator.Emit(OpCodes.Castclass, returnType);
                    else if (returnType == typeof(object))
                        ilGenerator.Emit(OpCodes.Box, method.ReturnType);
                    else if (method.ReturnType.IsClass)
                        ilGenerator.Emit(OpCodes.Unbox, returnType);
                }
                else
                    throw new Exception(string.Format("Cannot cast {0} to {1}", method.ReturnType.Name, returnType));


            ilGenerator.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(Expression.GetFuncType(new[] { typeof(object) }.Concat(delegParams).Concat(new[] {returnType}).ToArray()));

        }
    }
}
