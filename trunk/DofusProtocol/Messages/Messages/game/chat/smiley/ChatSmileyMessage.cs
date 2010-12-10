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
	
	public class ChatSmileyMessage : Message
	{
		public const uint protocolId = 801;
		internal Boolean _isInitialized = false;
		public int entityId = 0;
		public uint smileyId = 0;
		public uint accountId = 0;
		
		public ChatSmileyMessage()
		{
		}
		
		public ChatSmileyMessage(int arg1, uint arg2, uint arg3)
			: this()
		{
			initChatSmileyMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 801;
		}
		
		public ChatSmileyMessage initChatSmileyMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.entityId = arg1;
			this.smileyId = arg2;
			this.accountId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.entityId = 0;
			this.smileyId = 0;
			this.accountId = 0;
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ChatSmileyMessage(arg1);
		}
		
		public void serializeAs_ChatSmileyMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.entityId);
			if ( this.smileyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.smileyId + ") on element smileyId.");
			}
			arg1.WriteByte((byte)this.smileyId);
			if ( this.accountId < 0 )
			{
				throw new Exception("Forbidden value (" + this.accountId + ") on element accountId.");
			}
			arg1.WriteInt((int)this.accountId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatSmileyMessage(arg1);
		}
		
		public void deserializeAs_ChatSmileyMessage(BigEndianReader arg1)
		{
			this.entityId = (int)arg1.ReadInt();
			this.smileyId = (uint)arg1.ReadByte();
			if ( this.smileyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.smileyId + ") on element of ChatSmileyMessage.smileyId.");
			}
			this.accountId = (uint)arg1.ReadInt();
			if ( this.accountId < 0 )
			{
				throw new Exception("Forbidden value (" + this.accountId + ") on element of ChatSmileyMessage.accountId.");
			}
		}
		
	}
}
