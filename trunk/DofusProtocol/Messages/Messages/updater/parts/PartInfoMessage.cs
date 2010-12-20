using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartInfoMessage : Message
	{
		public const uint protocolId = 1507;
		internal Boolean _isInitialized = false;
		public ContentPart part;
		
		public PartInfoMessage()
		{
			this.part = new ContentPart();
		}
		
		public PartInfoMessage(ContentPart arg1)
			: this()
		{
			initPartInfoMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1507;
		}
		
		public PartInfoMessage initPartInfoMessage(ContentPart arg1 = null)
		{
			this.part = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.part = new ContentPart();
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
			this.serializeAs_PartInfoMessage(arg1);
		}
		
		public void serializeAs_PartInfoMessage(BigEndianWriter arg1)
		{
			this.part.serializeAs_ContentPart(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartInfoMessage(arg1);
		}
		
		public void deserializeAs_PartInfoMessage(BigEndianReader arg1)
		{
			this.part = new ContentPart();
			this.part.deserialize(arg1);
		}
		
	}
}
