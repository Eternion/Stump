#region License GNU GPL
// IPCMessageSerializer.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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
using System.IO;
using System.Linq;
using System.Reflection;
using ProtoBuf;
using ProtoBuf.Meta;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.IPC
{
    public class IPCMessageSerializer : Singleton<IPCMessageSerializer>
    {
        private int m_idCounter = 50;

        public IPCMessageSerializer()
        {
            Model = TypeModel.Create();
            RegisterMessages(Assembly.GetExecutingAssembly());
        }

        public RuntimeTypeModel Model
        {
            get;
            private set;
        }

        public void RegisterMessages(Assembly assembly)
        {
            foreach (var messageType in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(IPCMessage))))
            {
                if (messageType == typeof(IPCMessage))
                    continue;

                Model[typeof(IPCMessage)].AddSubType(m_idCounter++, messageType);
            }
        }

        public void RegisterMessage(Type type)
        {
            Model[typeof(IPCMessage)].AddSubType(m_idCounter++, type);
        }
        public void RegisterMessage(Type type, int id)
        {
            Model[typeof(IPCMessage)].AddSubType(id, type);
        }

        public IPCMessage Deserialize(byte[] buffer)
        {
            return Deserialize(buffer, 0, buffer.Length);
        }

        public IPCMessage Deserialize(byte[] buffer, int offset, int count)
        {
            return (IPCMessage)Model.DeserializeWithLengthPrefix(new MemoryStream(buffer, offset, count), null, typeof(IPCMessage), PrefixStyle.Base128, 20);
        }

        public byte[] Serialize(IPCMessage message)
        {
            var stream = new MemoryStream();
            Model.SerializeWithLengthPrefix(stream, message, typeof(IPCMessage), PrefixStyle.Base128, 20);
            return stream.ToArray();
        }
    }
}