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
	
	public class ExchangeWeightMessage : Message
	{
		public const uint protocolId = 5793;
		internal Boolean _isInitialized = false;
		public uint currentWeight = 0;
		public uint maxWeight = 0;
		
		public ExchangeWeightMessage()
		{
		}
		
		public ExchangeWeightMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeWeightMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5793;
		}
		
		public ExchangeWeightMessage initExchangeWeightMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.currentWeight = arg1;
			this.maxWeight = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.currentWeight = 0;
			this.maxWeight = 0;
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
			this.serializeAs_ExchangeWeightMessage(arg1);
		}
		
		public void serializeAs_ExchangeWeightMessage(BigEndianWriter arg1)
		{
			if ( this.currentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.currentWeight + ") on element currentWeight.");
			}
			arg1.WriteInt((int)this.currentWeight);
			if ( this.maxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxWeight + ") on element maxWeight.");
			}
			arg1.WriteInt((int)this.maxWeight);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeWeightMessage(arg1);
		}
		
		public void deserializeAs_ExchangeWeightMessage(BigEndianReader arg1)
		{
			this.currentWeight = (uint)arg1.ReadInt();
			if ( this.currentWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.currentWeight + ") on element of ExchangeWeightMessage.currentWeight.");
			}
			this.maxWeight = (uint)arg1.ReadInt();
			if ( this.maxWeight < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxWeight + ") on element of ExchangeWeightMessage.maxWeight.");
			}
		}
		
	}
}
