using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseGenericItemRemovedMessage : Message
	{
		public const uint protocolId = 5948;
		internal Boolean _isInitialized = false;
		public int objGenericId = 0;
		
		public ExchangeBidHouseGenericItemRemovedMessage()
		{
		}
		
		public ExchangeBidHouseGenericItemRemovedMessage(int arg1)
			: this()
		{
			initExchangeBidHouseGenericItemRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5948;
		}
		
		public ExchangeBidHouseGenericItemRemovedMessage initExchangeBidHouseGenericItemRemovedMessage(int arg1 = 0)
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
			this.serializeAs_ExchangeBidHouseGenericItemRemovedMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseGenericItemRemovedMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.objGenericId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseGenericItemRemovedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseGenericItemRemovedMessage(BigEndianReader arg1)
		{
			this.objGenericId = (int)arg1.ReadInt();
		}
		
	}
}
