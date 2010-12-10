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
	
	public class ExchangeSellMessage : Message
	{
		public const uint protocolId = 5778;
		internal Boolean _isInitialized = false;
		public uint objectToSellId = 0;
		public uint quantity = 0;
		
		public ExchangeSellMessage()
		{
		}
		
		public ExchangeSellMessage(uint arg1, uint arg2)
			: this()
		{
			initExchangeSellMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5778;
		}
		
		public ExchangeSellMessage initExchangeSellMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectToSellId = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectToSellId = 0;
			this.quantity = 0;
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
			this.serializeAs_ExchangeSellMessage(arg1);
		}
		
		public void serializeAs_ExchangeSellMessage(BigEndianWriter arg1)
		{
			if ( this.@objectToSellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToSellId + ") on element objectToSellId.");
			}
			arg1.WriteInt((int)this.@objectToSellId);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeSellMessage(arg1);
		}
		
		public void deserializeAs_ExchangeSellMessage(BigEndianReader arg1)
		{
			this.@objectToSellId = (uint)arg1.ReadInt();
			if ( this.@objectToSellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectToSellId + ") on element of ExchangeSellMessage.objectToSellId.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ExchangeSellMessage.quantity.");
			}
		}
		
	}
}
