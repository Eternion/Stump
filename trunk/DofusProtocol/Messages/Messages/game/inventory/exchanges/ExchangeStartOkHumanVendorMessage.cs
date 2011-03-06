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
