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
	
	public class ExchangeBidHouseItemAddOkMessage : Message
	{
		public const uint protocolId = 5945;
		internal Boolean _isInitialized = false;
		public ObjectItemToSellInBid itemInfo;
		
		public ExchangeBidHouseItemAddOkMessage()
		{
			this.itemInfo = new ObjectItemToSellInBid();
		}
		
		public ExchangeBidHouseItemAddOkMessage(ObjectItemToSellInBid arg1)
			: this()
		{
			initExchangeBidHouseItemAddOkMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5945;
		}
		
		public ExchangeBidHouseItemAddOkMessage initExchangeBidHouseItemAddOkMessage(ObjectItemToSellInBid arg1 = null)
		{
			this.itemInfo = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.itemInfo = new ObjectItemToSellInBid();
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
			this.serializeAs_ExchangeBidHouseItemAddOkMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseItemAddOkMessage(BigEndianWriter arg1)
		{
			this.itemInfo.serializeAs_ObjectItemToSellInBid(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseItemAddOkMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseItemAddOkMessage(BigEndianReader arg1)
		{
			this.itemInfo = new ObjectItemToSellInBid();
			this.itemInfo.deserialize(arg1);
		}
		
	}
}
