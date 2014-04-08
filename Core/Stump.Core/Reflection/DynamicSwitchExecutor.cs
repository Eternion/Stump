using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Stump.Core.Reflection
{
    class DynamicSwitchExecutor
    {
        private readonly Type m_type;
        private readonly List<SwitchCase> m_cases = new List<SwitchCase>();


        public void Add(int value, object instance, MethodInfo method)
        {
            m_cases.Add(new SwitchCase(value, instance, method));
        }

        public Tuple<Dictionary<int, int>, Delegate> Build(params Type[] delegParams)
        {
            var dico = new Dictionary<int, int>();
            var dynamicMethod = new DynamicMethod(string.Empty, null, delegParams);
            var ilGenerator = dynamicMethod.GetILGenerator();

            var endOfMethod = ilGenerator.DefineLabel();

            var labels = new Label[m_cases.Count];
            for (var i = 0; i < m_cases.Count; i++)
            {
                labels[i] = ilGenerator.DefineLabel();
                dico.Add(m_cases[i].Value, i);
            }

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Switch, labels);

            for (int i = 0; i < m_cases.Count; i++)
            {
                ilGenerator.MarkLabel(labels[i]);
                var methodParams = m_cases[i].Method.GetParameters().Select(p => p.ParameterType).ToArray();

                if (methodParams.Length != delegParams.Length - 1)
                    throw new Exception("Le nombre d'arguments passé ne correspond pas au nombre d'arguments de la methode");

                for (var j = 1; j < delegParams.Length; j++)
                {
                    ilGenerator.Emit(OpCodes.Ldarg, j);
                    if (delegParams[j] != methodParams[j - 1])
                        if (methodParams[j - 1].IsSubclassOf(delegParams[j]))
                            ilGenerator.Emit(methodParams[j - 1].IsClass ? OpCodes.Castclass : OpCodes.Unbox, methodParams[j - 1]);
                        else
                            throw new Exception("Impossible de réaliser un cast vers un object de ce type");
                }
                ilGenerator.Emit(OpCodes.Call, m_cases[i].Method);
                ilGenerator.Emit(OpCodes.Br_S, endOfMethod);
            }

            ilGenerator.MarkLabel(endOfMethod);
            ilGenerator.Emit(OpCodes.Ret);

            return new Tuple<Dictionary<int, int>, Delegate>(dico, dynamicMethod.CreateDelegate(Expression.GetActionType(delegParams)));
        }

        private class SwitchCase
        {
            public int Value { get; private set; }
            public object Instance { get; private set; }
            public MethodInfo Method { get; private set; }

            public SwitchCase(int value, object instance, MethodInfo method)
            {
                Value = value;
                Instance = instance;
                Method = method;
            }
        }
    }
}
