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
	
	public class GuildInvitedMessage : Message
	{
		public const uint protocolId = 5552;
		internal Boolean _isInitialized = false;
		public uint recruterId = 0;
		public String recruterName = "";
		public String guildName = "";
		
		public GuildInvitedMessage()
		{
		}
		
		public GuildInvitedMessage(uint arg1, String arg2, String arg3)
			: this()
		{
			initGuildInvitedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5552;
		}
		
		public GuildInvitedMessage initGuildInvitedMessage(uint arg1 = 0, String arg2 = "", String arg3 = "")
		{
			this.recruterId = arg1;
			this.recruterName = arg2;
			this.guildName = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.recruterId = 0;
			this.recruterName = "";
			this.guildName = "";
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
			this.serializeAs_GuildInvitedMessage(arg1);
		}
		
		public void serializeAs_GuildInvitedMessage(BigEndianWriter arg1)
		{
			if ( this.recruterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.recruterId + ") on element recruterId.");
			}
			arg1.WriteInt((int)this.recruterId);
			arg1.WriteUTF((string)this.recruterName);
			arg1.WriteUTF((string)this.guildName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInvitedMessage(arg1);
		}
		
		public void deserializeAs_GuildInvitedMessage(BigEndianReader arg1)
		{
			this.recruterId = (uint)arg1.ReadInt();
			if ( this.recruterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.recruterId + ") on element of GuildInvitedMessage.recruterId.");
			}
			this.recruterName = (String)arg1.ReadUTF();
			this.guildName = (String)arg1.ReadUTF();
		}
		
	}
}
