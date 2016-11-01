#region License GNU GPL

// DiscriminatorManager.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Database
{
    public class DiscriminatorManager<TValue> : DiscriminatorManager<TValue, string>
    {
    }

    public class DiscriminatorManager<TValue,TKey> : Singleton<DiscriminatorManager<TValue,TKey>>
    {

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private bool m_initialized;
        private readonly Dictionary<TKey, Delegate> m_constructors = new Dictionary<TKey, Delegate>();
        // todo  : manage assemblies correctly

        public void Initialize(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var attribute in type.GetCustomAttributes<DiscriminatorAttribute>())
                    {
                        if (attribute == null)
                            continue;

                        var targetType = attribute.BaseType;

                        if (targetType != typeof(TValue))
                            continue;

                        if (!(attribute.Discriminator is TKey))
                            continue;

                        var ctor = type.GetConstructor(attribute.CtorParameters);

                        if (ctor == null)
                        {
                            logger.Error($"No constructor for type {type}");
                            continue;
                        }

                        var del = ctor.CreateDelegate();
                        m_constructors.Add((TKey)attribute.Discriminator, del);
                        /*var parameters = new List<Type>();
                    parameters.AddRange(attribute.CtorParameters);
                    parameters.Add(type);
                    m_constructors.Add(attribute.Discriminator, Delegate.CreateDelegate(Expression.GetFuncType(parameters.ToArray()), del.Target, del.Method));*/
                    }
                }
            }
            finally
            {
                m_initialized = true;
            }
        }

        private void CheckBeforeGenerate(TKey discriminator, Assembly assembly)
        {
            if (!m_initialized)
                Initialize(assembly);

            if (!m_constructors.ContainsKey(discriminator))
                throw new Exception(string.Format("Type bound to discriminator '{0}' not found ({1})", discriminator, typeof(TValue)));

        }

        public TValue Generate<TArg>(TKey discriminator, TArg parameter)
        {
            CheckBeforeGenerate(discriminator, typeof(TValue).Assembly);

            return ((Func<TArg, TValue>)m_constructors[discriminator])(parameter);
        }

        public TValue Generate<TArg1, TArg2>(TKey discriminator, TArg1 parameter1, TArg2 parameter2)
        {
            CheckBeforeGenerate(discriminator, typeof(TValue).Assembly);

            return ( (Func<TArg1, TArg2, TValue>)m_constructors[discriminator] )(parameter1, parameter2);
        }

        public TValue Generate<TArg1, TArg2, TArg3>(TKey discriminator, TArg1 parameter1, TArg2 parameter2, TArg3 parameter3)
        {
            CheckBeforeGenerate(discriminator, typeof(TValue).Assembly);

            return ( (Func<TArg1, TArg2, TArg3, TValue>)m_constructors[discriminator] )(parameter1, parameter2, parameter3);
        }

        public TValue Generate<TArg1, TArg2, TArg3, TArg4>(TKey discriminator, TArg1 parameter1, TArg2 parameter2, TArg3 parameter3, TArg4 parameter4)
        {
            CheckBeforeGenerate(discriminator, typeof(TValue).Assembly);

            return ( (Func<TArg1, TArg2, TArg3, TArg4, TValue>)m_constructors[discriminator] )(parameter1, parameter2, parameter3, parameter4);
        }
    }
}