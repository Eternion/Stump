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
	
	public class ContactLookMessage : Message
	{
		public const uint protocolId = 5934;
		internal Boolean _isInitialized = false;
		public uint requestId = 0;
		public String playerName = "";
		public uint playerId = 0;
		public EntityLook look;
		
		public ContactLookMessage()
		{
			this.look = new EntityLook();
		}
		
		public ContactLookMessage(uint arg1, String arg2, uint arg3, EntityLook arg4)
			: this()
		{
			initContactLookMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5934;
		}
		
		public ContactLookMessage initContactLookMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, EntityLook arg4 = null)
		{
			this.requestId = arg1;
			this.playerName = arg2;
			this.playerId = arg3;
			this.look = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.requestId = 0;
			this.playerName = "";
			this.playerId = 0;
			this.look = new EntityLook();
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
			this.serializeAs_ContactLookMessage(arg1);
		}
		
		public void serializeAs_ContactLookMessage(BigEndianWriter arg1)
		{
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element requestId.");
			}
			arg1.WriteInt((int)this.requestId);
			arg1.WriteUTF((string)this.playerName);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
			this.look.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContactLookMessage(arg1);
		}
		
		public void deserializeAs_ContactLookMessage(BigEndianReader arg1)
		{
			this.requestId = (uint)arg1.ReadInt();
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element of ContactLookMessage.requestId.");
			}
			this.playerName = (String)arg1.ReadUTF();
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of ContactLookMessage.playerId.");
			}
			this.look = new EntityLook();
			this.look.deserialize(arg1);
		}
		
	}
}
