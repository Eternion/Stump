using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseInListRemovedMessage : Message
	{
		public const uint protocolId = 5950;
		internal Boolean _isInitialized = false;
		public int itemUID = 0;
		
		public ExchangeBidHouseInListRemovedMessage()
		{
		}
		
		public ExchangeBidHouseInListRemovedMessage(int arg1)
			: this()
		{
			initExchangeBidHouseInListRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5950;
		}
		
		public ExchangeBidHouseInListRemovedMessage initExchangeBidHouseInListRemovedMessage(int arg1 = 0)
		{
			this.itemUID = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.itemUID = 0;
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
			this.serializeAs_ExchangeBidHouseInListRemovedMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseInListRemovedMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.itemUID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseInListRemovedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseInListRemovedMessage(BigEndianReader arg1)
		{
			this.itemUID = (int)arg1.ReadInt();
		}
		
	}
}
