using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartInfoMessage : Message
	{
		public const uint protocolId = 1508;
		internal Boolean _isInitialized = false;
		public ContentPart part;
		public double installationPercent = 0;
		
		public PartInfoMessage()
		{
			this.part = new ContentPart();
		}
		
		public PartInfoMessage(ContentPart arg1, double arg2)
			: this()
		{
			initPartInfoMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1508;
		}
		
		public PartInfoMessage initPartInfoMessage(ContentPart arg1 = null, double arg2 = 0)
		{
			this.part = arg1;
			this.installationPercent = arg2;
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
			arg1.WriteFloat((uint)this.installationPercent);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartInfoMessage(arg1);
		}
		
		public void deserializeAs_PartInfoMessage(BigEndianReader arg1)
		{
			this.part = new ContentPart();
			this.part.deserialize(arg1);
			this.installationPercent = (double)arg1.ReadFloat();
		}
		
	}
}
