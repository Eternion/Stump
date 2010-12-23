// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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