using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeTypesItemsExchangerDescriptionForUserMessage : Message
	{
		public const uint protocolId = 5752;
		internal Boolean _isInitialized = false;
		public List<BidExchangerObjectInfo> itemTypeDescriptions;
		
		public ExchangeTypesItemsExchangerDescriptionForUserMessage()
		{
			this.itemTypeDescriptions = new List<BidExchangerObjectInfo>();
		}
		
		public ExchangeTypesItemsExchangerDescriptionForUserMessage(List<BidExchangerObjectInfo> arg1)
			: this()
		{
			initExchangeTypesItemsExchangerDescriptionForUserMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5752;
		}
		
		public ExchangeTypesItemsExchangerDescriptionForUserMessage initExchangeTypesItemsExchangerDescriptionForUserMessage(List<BidExchangerObjectInfo> arg1)
		{
			this.itemTypeDescriptions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.itemTypeDescriptions = new List<BidExchangerObjectInfo>();
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
			this.serializeAs_ExchangeTypesItemsExchangerDescriptionForUserMessage(arg1);
		}
		
		public void serializeAs_ExchangeTypesItemsExchangerDescriptionForUserMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.itemTypeDescriptions.Count);
			var loc1 = 0;
			while ( loc1 < this.itemTypeDescriptions.Count )
			{
				this.itemTypeDescriptions[loc1].serializeAs_BidExchangerObjectInfo(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeTypesItemsExchangerDescriptionForUserMessage(arg1);
		}
		
		public void deserializeAs_ExchangeTypesItemsExchangerDescriptionForUserMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new BidExchangerObjectInfo()) as BidExchangerObjectInfo).deserialize(arg1);
				this.itemTypeDescriptions.Add((BidExchangerObjectInfo)loc3);
				++loc2;
			}
		}
		
	}
}
