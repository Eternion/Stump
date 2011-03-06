using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HelloConnectMessage : Message
	{
		public const uint protocolId = 3;
		internal Boolean _isInitialized = false;
		public uint connectionType = 1;
		public String key = "";
		
		public HelloConnectMessage()
		{
		}
		
		public HelloConnectMessage(uint arg1, String arg2)
			: this()
		{
			initHelloConnectMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3;
		}
		
		public HelloConnectMessage initHelloConnectMessage(uint arg1 = 1, String arg2 = "")
		{
			this.connectionType = arg1;
			this.key = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.connectionType = 1;
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
			this.serializeAs_HelloConnectMessage(arg1);
		}
		
		public void serializeAs_HelloConnectMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.connectionType);
			arg1.WriteUTF((string)this.key);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HelloConnectMessage(arg1);
		}
		
		public void deserializeAs_HelloConnectMessage(BigEndianReader arg1)
		{
			this.connectionType = (uint)arg1.ReadByte();
			if ( this.connectionType < 0 )
			{
				throw new Exception("Forbidden value (" + this.connectionType + ") on element of HelloConnectMessage.connectionType.");
			}
			this.key = (String)arg1.ReadUTF();
		}
		
	}
}
