using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ConsoleMessage : Message
	{
		public const uint protocolId = 75;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		public String content = "";
		
		public ConsoleMessage()
		{
		}
		
		public ConsoleMessage(uint arg1, String arg2)
			: this()
		{
			initConsoleMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 75;
		}
		
		public ConsoleMessage initConsoleMessage(uint arg1 = 0, String arg2 = "")
		{
			this.type = arg1;
			this.content = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
			this.content = "";
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
			this.serializeAs_ConsoleMessage(arg1);
		}
		
		public void serializeAs_ConsoleMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
			arg1.WriteUTF((string)this.content);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ConsoleMessage(arg1);
		}
		
		public void deserializeAs_ConsoleMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of ConsoleMessage.type.");
			}
			this.content = (String)arg1.ReadUTF();
		}
		
	}
}
