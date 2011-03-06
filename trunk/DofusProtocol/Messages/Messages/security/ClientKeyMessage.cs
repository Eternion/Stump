using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ClientKeyMessage : Message
	{
		public const uint protocolId = 5607;
		internal Boolean _isInitialized = false;
		public String key = "";
		
		public ClientKeyMessage()
		{
		}
		
		public ClientKeyMessage(String arg1)
			: this()
		{
			initClientKeyMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5607;
		}
		
		public ClientKeyMessage initClientKeyMessage(String arg1 = "")
		{
			this.key = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.key = "";
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
			this.serializeAs_ClientKeyMessage(arg1);
		}
		
		public void serializeAs_ClientKeyMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.key);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ClientKeyMessage(arg1);
		}
		
		public void deserializeAs_ClientKeyMessage(BigEndianReader arg1)
		{
			this.key = (String)arg1.ReadUTF();
		}
		
	}
}
