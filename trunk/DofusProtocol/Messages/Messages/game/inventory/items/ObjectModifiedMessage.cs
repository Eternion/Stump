using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectModifiedMessage : Message
	{
		public const uint protocolId = 3029;
		internal Boolean _isInitialized = false;
		public ObjectItem @object;
		
		public ObjectModifiedMessage()
		{
			this.@object = new ObjectItem();
		}
		
		public ObjectModifiedMessage(ObjectItem arg1)
			: this()
		{
			initObjectModifiedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 3029;
		}
		
		public ObjectModifiedMessage initObjectModifiedMessage(ObjectItem arg1 = null)
		{
			this.@object = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@object = new ObjectItem();
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
			this.serializeAs_ObjectModifiedMessage(arg1);
		}
		
		public void serializeAs_ObjectModifiedMessage(BigEndianWriter arg1)
		{
			this.@object.serializeAs_ObjectItem(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectModifiedMessage(arg1);
		}
		
		public void deserializeAs_ObjectModifiedMessage(BigEndianReader arg1)
		{
			this.@object = new ObjectItem();
			this.@object.deserialize(arg1);
		}
		
	}
}
