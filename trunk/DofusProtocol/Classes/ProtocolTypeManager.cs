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
using System.Reflection;
using Stump.BaseCore.Framework.Reflection;
using Version = Stump.DofusProtocol.Classes.Version;

namespace Stump.DofusProtocol
{
    public static class ProtocolTypeManager
    {
        private static readonly Dictionary<uint, Type> m_types = new Dictionary<uint, Type>();
        private static readonly Dictionary<uint, Delegate> m_typesConstructors = new Dictionary<uint, Delegate>();

        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof (Version));

            foreach (Type type in asm.GetTypes())
            {
                FieldInfo fi = type.GetField("protocolId");

                if (fi != null)
                {
                    m_types.Add((uint) fi.GetValue(type), type);

                    var ctor = type.GetConstructor(new Type[0]);

                    if (ctor == null)
                        throw new Exception(string.Format("'{0}' doesn't implemented a parameterless constructor", type.ToString()));

                    m_typesConstructors.Add((uint) fi.GetValue(type), ctor.CreateDelegate(ctor.GetFuncType()));
                }
            }
        }

        /// <summary>
        ///   Gets instance of the type defined by id.
        /// </summary>
        /// <typeparam name = "T">Type.</typeparam>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static T GetInstance<T>(uint id) where T : class
        {
            if (!m_types.ContainsKey(id))
                throw new KeyNotFoundException(string.Format("Type <id:{0} doesn't exist", id));

            return m_typesConstructors[id].DynamicInvoke(new object[0]) as T;
        }
    }
}