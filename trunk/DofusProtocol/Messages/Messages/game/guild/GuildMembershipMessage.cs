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
	
	public class GuildMembershipMessage : GuildJoinedMessage
	{
		public const uint protocolId = 5835;
		internal Boolean _isInitialized = false;
		
		public GuildMembershipMessage()
		{
		}
		
		public GuildMembershipMessage(GuildInformations arg1, uint arg2)
			: this()
		{
			initGuildMembershipMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5835;
		}
		
		public GuildMembershipMessage initGuildMembershipMessage(GuildInformations arg1 = null, uint arg2 = 0)
		{
			base.initGuildJoinedMessage(arg1, arg2);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_GuildMembershipMessage(arg1);
		}
		
		public void serializeAs_GuildMembershipMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GuildJoinedMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildMembershipMessage(arg1);
		}
		
		public void deserializeAs_GuildMembershipMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
