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
	
	public class StatsUpgradeRequestMessage : Message
	{
		public const uint protocolId = 5610;
		internal Boolean _isInitialized = false;
		public uint statId = 11;
		public uint boostPoint = 0;
		
		public StatsUpgradeRequestMessage()
		{
		}
		
		public StatsUpgradeRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initStatsUpgradeRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5610;
		}
		
		public StatsUpgradeRequestMessage initStatsUpgradeRequestMessage(uint arg1 = 11, uint arg2 = 0)
		{
			this.statId = arg1;
			this.boostPoint = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.statId = 11;
			this.boostPoint = 0;
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
			this.serializeAs_StatsUpgradeRequestMessage(arg1);
		}
		
		public void serializeAs_StatsUpgradeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.statId);
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element boostPoint.");
			}
			arg1.WriteShort((short)this.boostPoint);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatsUpgradeRequestMessage(arg1);
		}
		
		public void deserializeAs_StatsUpgradeRequestMessage(BigEndianReader arg1)
		{
			this.statId = (uint)arg1.ReadByte();
			if ( this.statId < 0 )
			{
				throw new Exception("Forbidden value (" + this.statId + ") on element of StatsUpgradeRequestMessage.statId.");
			}
			this.boostPoint = (uint)arg1.ReadShort();
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element of StatsUpgradeRequestMessage.boostPoint.");
			}
		}
		
	}
}
