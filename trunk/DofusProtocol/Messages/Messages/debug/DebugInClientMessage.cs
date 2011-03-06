using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DebugInClientMessage : Message
	{
		public const uint protocolId = 6028;
		internal Boolean _isInitialized = false;
		public uint level = 0;
		public String message = "";
		
		public DebugInClientMessage()
		{
		}
		
		public DebugInClientMessage(uint arg1, String arg2)
			: this()
		{
			initDebugInClientMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6028;
		}
		
		public DebugInClientMessage initDebugInClientMessage(uint arg1 = 0, String arg2 = "")
		{
			this.level = arg1;
			this.message = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.level = 0;
			this.message = "";
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
			this.serializeAs_DebugInClientMessage(arg1);
		}
		
		public void serializeAs_DebugInClientMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.level);
			arg1.WriteUTF((string)this.message);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DebugInClientMessage(arg1);
		}
		
		public void deserializeAs_DebugInClientMessage(BigEndianReader arg1)
		{
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of DebugInClientMessage.level.");
			}
			this.message = (String)arg1.ReadUTF();
		}
		
	}
}
