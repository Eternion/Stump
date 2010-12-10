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
	
	public class GuildMemberOnlineStatusMessage : Message
	{
		public const uint protocolId = 6061;
		internal Boolean _isInitialized = false;
		public uint memberId = 0;
		public Boolean online = false;
		
		public GuildMemberOnlineStatusMessage()
		{
		}
		
		public GuildMemberOnlineStatusMessage(uint arg1, Boolean arg2)
			: this()
		{
			initGuildMemberOnlineStatusMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6061;
		}
		
		public GuildMemberOnlineStatusMessage initGuildMemberOnlineStatusMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.memberId = arg1;
			this.online = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.memberId = 0;
			this.online = false;
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
			this.serializeAs_GuildMemberOnlineStatusMessage(arg1);
		}
		
		public void serializeAs_GuildMemberOnlineStatusMessage(BigEndianWriter arg1)
		{
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element memberId.");
			}
			arg1.WriteInt((int)this.memberId);
			arg1.WriteBoolean(this.online);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildMemberOnlineStatusMessage(arg1);
		}
		
		public void deserializeAs_GuildMemberOnlineStatusMessage(BigEndianReader arg1)
		{
			this.memberId = (uint)arg1.ReadInt();
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element of GuildMemberOnlineStatusMessage.memberId.");
			}
			this.online = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
