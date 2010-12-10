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
using Stump.BaseCore.Framework.IO;

namespace Stump.DofusProtocol.Messages
{
    public static class MessageReceiver
    {
        private static readonly Dictionary<uint, Type> Messages = new Dictionary<uint, Type>();

        private static bool m_initialized;

        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            Assembly asm = Assembly.GetAssembly(typeof (IdentificationMessage));

            foreach (Type type in asm.GetTypes())
            {
                FieldInfo fi = type.GetField("protocolId");

                if (fi != null)
                    Messages.Add((uint) fi.GetValue(type), type);
            }

            m_initialized = true;
        }

        /// <summary>
        ///   Gets instance of the message defined by id thanks to BigEndianReader.
        /// </summary>
        /// <param name = "id">id.</param>
        /// <returns></returns>
        public static Message GetMessage(uint id, BigEndianReader reader)
        {
            if (!m_initialized)
                Initialize();

            if (!Messages.ContainsKey(id))
                throw new KeyNotFoundException("This Message doesn't exist");

            Type type = Messages[id];
            ConstructorInfo ci = type.GetConstructor(new Type[0] {});

            if (ci == null)
                throw new Exception("This Message doesn't implement parameterless's constructor");

            var message = ci.Invoke(new object[0] {}) as Message;

            message.unpack(reader, 0);
            return message;
        }
    }
}