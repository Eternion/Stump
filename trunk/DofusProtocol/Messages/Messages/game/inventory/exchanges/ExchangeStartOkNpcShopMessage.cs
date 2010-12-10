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
	
	public class ExchangeStartOkNpcShopMessage : Message
	{
		public const uint protocolId = 5761;
		internal Boolean _isInitialized = false;
		public int npcSellerId = 0;
		public uint tokenId = 0;
		public List<ObjectItemToSellInNpcShop> objectsInfos;
		
		public ExchangeStartOkNpcShopMessage()
		{
			this.@objectsInfos = new List<ObjectItemToSellInNpcShop>();
		}
		
		public ExchangeStartOkNpcShopMessage(int arg1, uint arg2, List<ObjectItemToSellInNpcShop> arg3)
			: this()
		{
			initExchangeStartOkNpcShopMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5761;
		}
		
		public ExchangeStartOkNpcShopMessage initExchangeStartOkNpcShopMessage(int arg1 = 0, uint arg2 = 0, List<ObjectItemToSellInNpcShop> arg3 = null)
		{
			this.npcSellerId = arg1;
			this.tokenId = arg2;
			this.@objectsInfos = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.npcSellerId = 0;
			this.tokenId = 0;
			this.@objectsInfos = new List<ObjectItemToSellInNpcShop>();
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
			this.serializeAs_ExchangeStartOkNpcShopMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkNpcShopMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.npcSellerId);
			if ( this.tokenId < 0 )
			{
				throw new Exception("Forbidden value (" + this.tokenId + ") on element tokenId.");
			}
			arg1.WriteInt((int)this.tokenId);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemToSellInNpcShop(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkNpcShopMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkNpcShopMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.npcSellerId = (int)arg1.ReadInt();
			this.tokenId = (uint)arg1.ReadInt();
			if ( this.tokenId < 0 )
			{
				throw new Exception("Forbidden value (" + this.tokenId + ") on element of ExchangeStartOkNpcShopMessage.tokenId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemToSellInNpcShop()) as ObjectItemToSellInNpcShop).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemToSellInNpcShop)loc3);
				++loc2;
			}
		}
		
	}
}
