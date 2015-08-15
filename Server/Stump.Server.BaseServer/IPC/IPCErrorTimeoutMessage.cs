#region License GNU GPL
// IPCErrorTimeoutMessage.cs
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
using ProtoBuf;

namespace Stump.Server.BaseServer.IPC
{
    public class IPCErrorTimeoutMessage : IPCMessage, IIPCErrorMessage
    {
        public IPCErrorTimeoutMessage()
        {
        }

        public IPCErrorTimeoutMessage(string message)
        {
            Message = message;
        }

        public IPCErrorTimeoutMessage(string message, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
        }

        [ProtoMember(2)]
        public string Message
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string StackTrace
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Message + " " + StackTrace;
        }

    }
}