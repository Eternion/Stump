using System;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Actions
{
    public class ActionBaseArgument
    {
        private readonly Stack<object> m_arguments;

        public ActionBaseArgument(params object[] arguments)
        {
            m_arguments = new Stack<object>(arguments);
        }

        public int Count
        {
            get { return m_arguments.Count; }
        }

        public T Next<T>()
        {
            object arg = m_arguments.Pop();

            if (!(arg is T))
                throw new ArgumentException(string.Format("The next argument is not of type {0}", typeof(T).Name));

            return (T)arg;
        }
    }
}