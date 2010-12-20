using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicWhoIsNoMatchMessage : Message
	{
		public const uint protocolId = 179;
		internal Boolean _isInitialized = false;
		public String search = "";
		
		public BasicWhoIsNoMatchMessage()
		{
		}
		
		public BasicWhoIsNoMatchMessage(String arg1)
			: this()
		{
			initBasicWhoIsNoMatchMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 179;
		}
		
		public BasicWhoIsNoMatchMessage initBasicWhoIsNoMatchMessage(String arg1 = "")
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
			this.serializeAs_BasicWhoIsNoMatchMessage(arg1);
		}
		
		public void serializeAs_BasicWhoIsNoMatchMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.search);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicWhoIsNoMatchMessage(arg1);
		}
		
		public void deserializeAs_BasicWhoIsNoMatchMessage(BigEndianReader arg1)
		{
			this.search = (String)arg1.ReadUTF();
		}
		
	}
}
