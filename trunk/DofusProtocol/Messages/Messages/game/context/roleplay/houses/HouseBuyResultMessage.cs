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
	
	public class HouseBuyResultMessage : Message
	{
		public const uint protocolId = 5735;
		internal Boolean _isInitialized = false;
		public uint houseId = 0;
		public Boolean bought = false;
		public uint realPrice = 0;
		
		public HouseBuyResultMessage()
		{
		}
		
		public HouseBuyResultMessage(uint arg1, Boolean arg2, uint arg3)
			: this()
		{
			initHouseBuyResultMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5735;
		}
		
		public HouseBuyResultMessage initHouseBuyResultMessage(uint arg1 = 0, Boolean arg2 = false, uint arg3 = 0)
		{
			this.houseId = arg1;
			this.bought = arg2;
			this.realPrice = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.houseId = 0;
			this.bought = false;
			this.realPrice = 0;
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
			this.serializeAs_HouseBuyResultMessage(arg1);
		}
		
		public void serializeAs_HouseBuyResultMessage(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
			arg1.WriteBoolean(this.bought);
			if ( this.realPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.realPrice + ") on element realPrice.");
			}
			arg1.WriteInt((int)this.realPrice);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseBuyResultMessage(arg1);
		}
		
		public void deserializeAs_HouseBuyResultMessage(BigEndianReader arg1)
		{
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseBuyResultMessage.houseId.");
			}
			this.bought = (Boolean)arg1.ReadBoolean();
			this.realPrice = (uint)arg1.ReadInt();
			if ( this.realPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.realPrice + ") on element of HouseBuyResultMessage.realPrice.");
			}
		}
		
	}
}
