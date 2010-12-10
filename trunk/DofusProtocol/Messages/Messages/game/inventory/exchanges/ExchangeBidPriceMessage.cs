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
	
	public class ExchangeBidPriceMessage : Message
	{
		public const uint protocolId = 5755;
		internal Boolean _isInitialized = false;
		public uint genericId = 0;
		public uint averagePrice = 0;
		
		public ExchangeBidPriceMessage()
		{
		}
		
		public ExchangeBidPriceMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeBidPriceMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5755;
		}
		
		public ExchangeBidPriceMessage initExchangeBidPriceMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.genericId = arg1;
			this.averagePrice = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.genericId = 0;
			this.averagePrice = 0;
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
			this.serializeAs_ExchangeBidPriceMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidPriceMessage(BigEndianWriter arg1)
		{
			if ( this.genericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genericId + ") on element genericId.");
			}
			arg1.WriteInt((int)this.genericId);
			if ( this.averagePrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.averagePrice + ") on element averagePrice.");
			}
			arg1.WriteInt((int)this.averagePrice);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidPriceMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidPriceMessage(BigEndianReader arg1)
		{
			this.genericId = (uint)arg1.ReadInt();
			if ( this.genericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genericId + ") on element of ExchangeBidPriceMessage.genericId.");
			}
			this.averagePrice = (uint)arg1.ReadInt();
			if ( this.averagePrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.averagePrice + ") on element of ExchangeBidPriceMessage.averagePrice.");
			}
		}
		
	}
}
