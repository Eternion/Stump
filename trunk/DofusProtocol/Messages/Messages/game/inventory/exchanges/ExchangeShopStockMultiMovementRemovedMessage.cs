using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeShopStockMultiMovementRemovedMessage : Message
	{
		public const uint protocolId = 6037;
		internal Boolean _isInitialized = false;
		public List<uint> objectIdList;
		
		public ExchangeShopStockMultiMovementRemovedMessage()
		{
			this.@objectIdList = new List<uint>();
		}
		
		public ExchangeShopStockMultiMovementRemovedMessage(List<uint> arg1)
			: this()
		{
			initExchangeShopStockMultiMovementRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6037;
		}
		
		public ExchangeShopStockMultiMovementRemovedMessage initExchangeShopStockMultiMovementRemovedMessage(List<uint> arg1)
		{
			this.@objectIdList = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectIdList = new List<uint>();
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
			this.serializeAs_ExchangeShopStockMultiMovementRemovedMessage(arg1);
		}
		
		public void serializeAs_ExchangeShopStockMultiMovementRemovedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectIdList.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectIdList.Count )
			{
				if ( this.@objectIdList[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.@objectIdList[loc1] + ") on element 1 (starting at 1) of objectIdList.");
				}
				arg1.WriteInt((int)this.@objectIdList[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeShopStockMultiMovementRemovedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeShopStockMultiMovementRemovedMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of objectIdList.");
				}
				this.@objectIdList.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
