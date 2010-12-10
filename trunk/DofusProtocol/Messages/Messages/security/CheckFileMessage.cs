using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CheckFileMessage : Message
	{
		public const uint protocolId = 6156;
		internal Boolean _isInitialized = false;
		public String filenameHash = "";
		public uint type = 0;
		public String value = "";
		
		public CheckFileMessage()
		{
		}
		
		public CheckFileMessage(String arg1, uint arg2, String arg3)
			: this()
		{
			initCheckFileMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6156;
		}
		
		public CheckFileMessage initCheckFileMessage(String arg1 = "", uint arg2 = 0, String arg3 = "")
		{
			this.filenameHash = arg1;
			this.type = arg2;
			this.value = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.filenameHash = "";
			this.type = 0;
			this.value = "";
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
			this.serializeAs_CheckFileMessage(arg1);
		}
		
		public void serializeAs_CheckFileMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.filenameHash);
			arg1.WriteByte((byte)this.type);
			arg1.WriteUTF((string)this.value);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CheckFileMessage(arg1);
		}
		
		public void deserializeAs_CheckFileMessage(BigEndianReader arg1)
		{
			this.filenameHash = (String)arg1.ReadUTF();
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of CheckFileMessage.type.");
			}
			this.value = (String)arg1.ReadUTF();
		}
		
	}
}
