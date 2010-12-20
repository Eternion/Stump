using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CheckFileRequestMessage : Message
	{
		public const uint protocolId = 6154;
		internal Boolean _isInitialized = false;
		public String filename = "";
		public uint type = 0;
		
		public CheckFileRequestMessage()
		{
		}
		
		public CheckFileRequestMessage(String arg1, uint arg2)
			: this()
		{
			initCheckFileRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6154;
		}
		
		public CheckFileRequestMessage initCheckFileRequestMessage(String arg1 = "", uint arg2 = 0)
		{
			this.filename = arg1;
			this.type = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.filename = "";
			this.type = 0;
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
			this.serializeAs_CheckFileRequestMessage(arg1);
		}
		
		public void serializeAs_CheckFileRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.filename);
			arg1.WriteByte((byte)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CheckFileRequestMessage(arg1);
		}
		
		public void deserializeAs_CheckFileRequestMessage(BigEndianReader arg1)
		{
			this.filename = (String)arg1.ReadUTF();
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of CheckFileRequestMessage.type.");
			}
		}
		
	}
}
