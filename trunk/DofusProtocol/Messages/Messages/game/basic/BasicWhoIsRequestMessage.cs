using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicWhoIsRequestMessage : Message
	{
		public const uint protocolId = 181;
		internal Boolean _isInitialized = false;
		public String search = "";
		
		public BasicWhoIsRequestMessage()
		{
		}
		
		public BasicWhoIsRequestMessage(String arg1)
			: this()
		{
			initBasicWhoIsRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 181;
		}
		
		public BasicWhoIsRequestMessage initBasicWhoIsRequestMessage(String arg1 = "")
		{
			this.search = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.search = "";
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
			this.serializeAs_BasicWhoIsRequestMessage(arg1);
		}
		
		public void serializeAs_BasicWhoIsRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.search);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicWhoIsRequestMessage(arg1);
		}
		
		public void deserializeAs_BasicWhoIsRequestMessage(BigEndianReader arg1)
		{
			this.search = (String)arg1.ReadUTF();
		}
		
	}
}
