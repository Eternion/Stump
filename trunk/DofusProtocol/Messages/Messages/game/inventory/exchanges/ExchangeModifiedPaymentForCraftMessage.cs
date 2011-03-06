using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeModifiedPaymentForCraftMessage : Message
	{
		public const uint protocolId = 5832;
		internal Boolean _isInitialized = false;
		public Boolean onlySuccess = false;
		public ObjectItemNotInContainer @object;
		
		public ExchangeModifiedPaymentForCraftMessage()
		{
			this.@object = new ObjectItemNotInContainer();
		}
		
		public ExchangeModifiedPaymentForCraftMessage(Boolean arg1, ObjectItemNotInContainer arg2)
			: this()
		{
			initExchangeModifiedPaymentForCraftMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5832;
		}
		
		public ExchangeModifiedPaymentForCraftMessage initExchangeModifiedPaymentForCraftMessage(Boolean arg1 = false, ObjectItemNotInContainer arg2 = null)
		{
			this.onlySuccess = arg1;
			this.@object = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.onlySuccess = false;
			this.@object = new ObjectItemNotInContainer();
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
			this.serializeAs_ExchangeModifiedPaymentForCraftMessage(arg1);
		}
		
		public void serializeAs_ExchangeModifiedPaymentForCraftMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.onlySuccess);
			this.@object.serializeAs_ObjectItemNotInContainer(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeModifiedPaymentForCraftMessage(arg1);
		}
		
		public void deserializeAs_ExchangeModifiedPaymentForCraftMessage(BigEndianReader arg1)
		{
			this.onlySuccess = (Boolean)arg1.ReadBoolean();
			this.@object = new ObjectItemNotInContainer();
			this.@object.deserialize(arg1);
		}
		
	}
}
