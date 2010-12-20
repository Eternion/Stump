using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StatedElementUpdatedMessage : Message
	{
		public const uint protocolId = 5709;
		internal Boolean _isInitialized = false;
		public StatedElement statedElement;
		
		public StatedElementUpdatedMessage()
		{
			this.statedElement = new StatedElement();
		}
		
		public StatedElementUpdatedMessage(StatedElement arg1)
			: this()
		{
			initStatedElementUpdatedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5709;
		}
		
		public StatedElementUpdatedMessage initStatedElementUpdatedMessage(StatedElement arg1 = null)
		{
			this.statedElement = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.statedElement = new StatedElement();
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
			this.serializeAs_StatedElementUpdatedMessage(arg1);
		}
		
		public void serializeAs_StatedElementUpdatedMessage(BigEndianWriter arg1)
		{
			this.statedElement.serializeAs_StatedElement(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatedElementUpdatedMessage(arg1);
		}
		
		public void deserializeAs_StatedElementUpdatedMessage(BigEndianReader arg1)
		{
			this.statedElement = new StatedElement();
			this.statedElement.deserialize(arg1);
		}
		
	}
}
