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
	
	public class ExchangeStartedBidSellerMessage : Message
	{
		public const uint protocolId = 5905;
		internal Boolean _isInitialized = false;
		public SellerBuyerDescriptor sellerDescriptor;
		public List<ObjectItemToSellInBid> objectsInfos;
		
		public ExchangeStartedBidSellerMessage()
		{
			this.sellerDescriptor = new SellerBuyerDescriptor();
			this.@objectsInfos = new List<ObjectItemToSellInBid>();
		}
		
		public ExchangeStartedBidSellerMessage(SellerBuyerDescriptor arg1, List<ObjectItemToSellInBid> arg2)
			: this()
		{
			initExchangeStartedBidSellerMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5905;
		}
		
		public ExchangeStartedBidSellerMessage initExchangeStartedBidSellerMessage(SellerBuyerDescriptor arg1 = null, List<ObjectItemToSellInBid> arg2 = null)
		{
			this.sellerDescriptor = arg1;
			this.@objectsInfos = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sellerDescriptor = new SellerBuyerDescriptor();
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
			this.serializeAs_ExchangeStartedBidSellerMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedBidSellerMessage(BigEndianWriter arg1)
		{
			this.sellerDescriptor.serializeAs_SellerBuyerDescriptor(arg1);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemToSellInBid(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedBidSellerMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedBidSellerMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.sellerDescriptor = new SellerBuyerDescriptor();
			this.sellerDescriptor.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSellInBid()) as ObjectItemToSellInBid).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemToSellInBid)loc3);
				++loc2;
			}
		}
		
	}
}
