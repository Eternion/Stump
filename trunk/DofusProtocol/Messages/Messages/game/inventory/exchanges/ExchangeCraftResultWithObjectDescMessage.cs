using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeCraftResultWithObjectDescMessage : ExchangeCraftResultMessage
	{
		public const uint protocolId = 5999;
		internal Boolean _isInitialized = false;
		public ObjectItemNotInContainer objectInfo;
		
		public ExchangeCraftResultWithObjectDescMessage()
		{
			this.@objectInfo = new ObjectItemNotInContainer();
		}
		
		public ExchangeCraftResultWithObjectDescMessage(uint arg1, ObjectItemNotInContainer arg2)
			: this()
		{
			initExchangeCraftResultWithObjectDescMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5999;
		}
		
		public ExchangeCraftResultWithObjectDescMessage initExchangeCraftResultWithObjectDescMessage(uint arg1 = 0, ObjectItemNotInContainer arg2 = null)
		{
			base.initExchangeCraftResultMessage(arg1);
			this.@objectInfo = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objectInfo = new ObjectItemNotInContainer();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ExchangeCraftResultWithObjectDescMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftResultWithObjectDescMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeCraftResultMessage(arg1);
			this.@objectInfo.serializeAs_ObjectItemNotInContainer(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftResultWithObjectDescMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftResultWithObjectDescMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.@objectInfo = new ObjectItemNotInContainer();
			this.@objectInfo.deserialize(arg1);
		}
		
	}
}
