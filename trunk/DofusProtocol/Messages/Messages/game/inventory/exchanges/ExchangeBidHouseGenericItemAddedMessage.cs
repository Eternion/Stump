using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseGenericItemAddedMessage : Message
	{
		public const uint protocolId = 5947;
		internal Boolean _isInitialized = false;
		public int objGenericId = 0;
		
		public ExchangeBidHouseGenericItemAddedMessage()
		{
		}
		
		public ExchangeBidHouseGenericItemAddedMessage(int arg1)
			: this()
		{
			initExchangeBidHouseGenericItemAddedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5947;
		}
		
		public ExchangeBidHouseGenericItemAddedMessage initExchangeBidHouseGenericItemAddedMessage(int arg1 = 0)
		{
			this.objGenericId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.objGenericId = 0;
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
			this.serializeAs_ExchangeBidHouseGenericItemAddedMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseGenericItemAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.objGenericId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseGenericItemAddedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseGenericItemAddedMessage(BigEndianReader arg1)
		{
			this.objGenericId = (int)arg1.ReadInt();
		}
		
	}
}
