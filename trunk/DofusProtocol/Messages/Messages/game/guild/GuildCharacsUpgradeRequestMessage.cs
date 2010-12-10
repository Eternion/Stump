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
	
	public class GuildCharacsUpgradeRequestMessage : Message
	{
		public const uint protocolId = 5706;
		internal Boolean _isInitialized = false;
		public uint charaTypeTarget = 0;
		
		public GuildCharacsUpgradeRequestMessage()
		{
		}
		
		public GuildCharacsUpgradeRequestMessage(uint arg1)
			: this()
		{
			initGuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5706;
		}
		
		public GuildCharacsUpgradeRequestMessage initGuildCharacsUpgradeRequestMessage(uint arg1 = 0)
		{
			this.charaTypeTarget = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.charaTypeTarget = 0;
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
			this.serializeAs_GuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public void serializeAs_GuildCharacsUpgradeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.charaTypeTarget);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildCharacsUpgradeRequestMessage(BigEndianReader arg1)
		{
			this.charaTypeTarget = (uint)arg1.ReadByte();
			if ( this.charaTypeTarget < 0 )
			{
				throw new Exception("Forbidden value (" + this.charaTypeTarget + ") on element of GuildCharacsUpgradeRequestMessage.charaTypeTarget.");
			}
		}
		
	}
}
