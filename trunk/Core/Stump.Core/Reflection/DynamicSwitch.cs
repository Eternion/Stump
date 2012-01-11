using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Stump.Core.Reflection
{
    public class DynamicSwitch<T>
    {
        private readonly Type m_typeT;
        private readonly List<SwitchCase<T>> m_cases = new List<SwitchCase<T>>();

        public DynamicSwitch()
        {
            m_typeT = typeof(T);
        }


        public void Add(T value, object obj, MethodInfo method)
        {
            m_cases.Add(new SwitchCase<T>(value, obj, method));
        }


        public Func<T, T1, object> Build<T1>()
        {
            var typeT1 = typeof(T1);
            /* This is the value for go to the correct case */
            var switchedValue = Expression.Parameter(m_typeT);
            /* This is the value passed to the correct case */
            var value = Expression.Parameter(typeT1);

            var cases = new List<SwitchCase>();
            
            foreach (var @case in m_cases)
            {
                /* If not right type try to Cast the arg to correct method type */
                var methodParam = @case.Method.GetParameters()[0].ParameterType;
                Expression arg = methodParam == typeT1 ? value : Expression.TypeAs(value, methodParam) as Expression;
                /* Call method */
                var caseBody = Expression.Call(null, @case.Method, arg);
                /* Cast return type if different that needed */
                var body = Expression.TypeAs(caseBody, typeof(object)) as Expression;
                /* Add the case */
                cases.Add(Expression.SwitchCase(body, Expression.Constant(@case.Value)));
            }

            var @switch = Expression.Switch(switchedValue, Expression.Constant(null, typeof(Object)), cases.ToArray());

            Expression func = @switch;

            while (func.CanReduce)
                func = func.Reduce();

            return Expression.Lambda<Func<T, T1, object>>(func, switchedValue, value).Compile();
        }

        private class SwitchCase<TC>
        {
            public TC Value { get; private set; }
            public object Object { get; private set; }
            public MethodInfo Method { get; private set; }

            public SwitchCase(TC value, object objet, MethodInfo method)
            {
                Value = value;
                Object = objet;
                Method = method;
            }
        }
    }
}
