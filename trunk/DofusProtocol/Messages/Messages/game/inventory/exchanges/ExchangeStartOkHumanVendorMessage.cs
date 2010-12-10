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
	
	public class ExchangeStartOkHumanVendorMessage : Message
	{
		public const uint protocolId = 5767;
		internal Boolean _isInitialized = false;
		public uint sellerId = 0;
		public List<ObjectItemToSellInHumanVendorShop> objectsInfos;
		
		public ExchangeStartOkHumanVendorMessage()
		{
			this.@objectsInfos = new List<ObjectItemToSellInHumanVendorShop>();
		}
		
		public ExchangeStartOkHumanVendorMessage(uint arg1, List<ObjectItemToSellInHumanVendorShop> arg2)
			: this()
		{
			initExchangeStartOkHumanVendorMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5767;
		}
		
		public ExchangeStartOkHumanVendorMessage initExchangeStartOkHumanVendorMessage(uint arg1 = 0, List<ObjectItemToSellInHumanVendorShop> arg2 = null)
		{
			this.sellerId = arg1;
			this.@objectsInfos = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sellerId = 0;
			this.@objectsInfos = new List<ObjectItemToSellInHumanVendorShop>();
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
			this.serializeAs_ExchangeStartOkHumanVendorMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkHumanVendorMessage(BigEndianWriter arg1)
		{
			if ( this.sellerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellerId + ") on element sellerId.");
			}
			arg1.WriteInt((int)this.sellerId);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemToSellInHumanVendorShop(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkHumanVendorMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkHumanVendorMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.sellerId = (uint)arg1.ReadInt();
			if ( this.sellerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellerId + ") on element of ExchangeStartOkHumanVendorMessage.sellerId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSellInHumanVendorShop()) as ObjectItemToSellInHumanVendorShop).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemToSellInHumanVendorShop)loc3);
				++loc2;
			}
		}
		
	}
}
