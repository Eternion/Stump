#region License GNU GPL
// BanIPMessage.cs
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
using ProtoBuf;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class BanIPMessage :IPCMessage
    {
        public BanIPMessage()
        {
            
        }
        [ProtoMember(2)]
        public string IPRange
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public DateTime? BanEndDate
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string BanReason
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public int? BannerAccountId
        {
            get;
            set;
        }
    }
}