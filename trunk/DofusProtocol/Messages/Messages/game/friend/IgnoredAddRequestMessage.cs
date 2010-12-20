using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IgnoredAddRequestMessage : Message
	{
		public const uint protocolId = 5673;
		internal Boolean _isInitialized = false;
		public String name = "";
		public Boolean session = false;
		
		public IgnoredAddRequestMessage()
		{
		}
		
		public IgnoredAddRequestMessage(String arg1, Boolean arg2)
			: this()
		{
			initIgnoredAddRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5673;
		}
		
		public IgnoredAddRequestMessage initIgnoredAddRequestMessage(String arg1 = "", Boolean arg2 = false)
		{
			this.name = arg1;
			this.session = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
			this.session = false;
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
			this.serializeAs_IgnoredAddRequestMessage(arg1);
		}
		
		public void serializeAs_IgnoredAddRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			arg1.WriteBoolean(this.session);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredAddRequestMessage(arg1);
		}
		
		public void deserializeAs_IgnoredAddRequestMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.session = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
