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
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatClientPrivateWithObjectMessage : ChatClientPrivateMessage
	{
		public const uint protocolId = 852;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objects;
		
		public ChatClientPrivateWithObjectMessage()
		{
			this.@objects = new List<ObjectItem>();
		}
		
		public ChatClientPrivateWithObjectMessage(String arg1, String arg2, List<ObjectItem> arg3)
			: this()
		{
			initChatClientPrivateWithObjectMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 852;
		}
		
		public ChatClientPrivateWithObjectMessage initChatClientPrivateWithObjectMessage(String arg1 = "", String arg2 = "", List<ObjectItem> arg3 = null)
		{
			base.initChatClientPrivateMessage(arg1, arg2);
			this.@objects = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objects = new List<ObjectItem>();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ChatClientPrivateWithObjectMessage(arg1);
		}
		
		public void serializeAs_ChatClientPrivateWithObjectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatClientPrivateMessage(arg1);
			arg1.WriteShort((short)this.@objects.Count);
			var loc1 = 0;
			while ( loc1 < this.@objects.Count )
			{
				this.@objects[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatClientPrivateWithObjectMessage(arg1);
		}
		
		public void deserializeAs_ChatClientPrivateWithObjectMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objects.Add((ObjectItem)loc3);
				++loc2;
			}
		}
		
	}
}
