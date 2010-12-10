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
using Version = Stump.DofusProtocol.Classes.Version;

namespace Stump.DofusProtocol
{
    public static class ProtocolTypeManager
    {
        private static readonly Dictionary<uint, Type> m_types = new Dictionary<uint, Type>();

        private static bool m_initialized;

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
                    m_types.Add((uint) fi.GetValue(type), type);
            }

            m_initialized = true;
        }

        /// <summary>
        ///   Gets instance of the type defined by id.
        /// </summary>
        /// <typeparam name = "T">Type.</typeparam>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static T GetInstance<T>(uint id) where T : class
        {
            if (!m_initialized)
                Initialize();

            if (!m_types.ContainsKey(id))
                throw new KeyNotFoundException("Le Type n'existe pas");

            Type type = m_types[id];

            if ((!type.IsSubclassOf(typeof (T))) && (!typeof (T).IsEquivalentTo(type)))
            {
                throw new Exception("Le type n'hérite pas de la classe");
            }
            ConstructorInfo ci = type.GetConstructor(new Type[0] {});

            if (ci == null)
                throw new Exception("Le type n'implémente pas de constructeur sans paramètres");

            return ci.Invoke(new object[0] {}) as T;
        }
    }
}