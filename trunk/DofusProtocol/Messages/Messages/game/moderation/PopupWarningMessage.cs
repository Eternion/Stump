using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PopupWarningMessage : Message
	{
		public const uint protocolId = 6134;
		internal Boolean _isInitialized = false;
		public uint lockDuration = 0;
		public String author = "";
		public String content = "";
		
		public PopupWarningMessage()
		{
		}
		
		public PopupWarningMessage(uint arg1, String arg2, String arg3)
			: this()
		{
			initPopupWarningMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6134;
		}
		
		public PopupWarningMessage initPopupWarningMessage(uint arg1 = 0, String arg2 = "", String arg3 = "")
		{
			this.lockDuration = arg1;
			this.author = arg2;
			this.content = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.lockDuration = 0;
			this.author = "";
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
			this.serializeAs_PopupWarningMessage(arg1);
		}
		
		public void serializeAs_PopupWarningMessage(BigEndianWriter arg1)
		{
			if ( this.lockDuration < 0 || this.lockDuration > 255 )
			{
				throw new Exception("Forbidden value (" + this.lockDuration + ") on element lockDuration.");
			}
			arg1.WriteByte((byte)this.lockDuration);
			arg1.WriteUTF((string)this.author);
			arg1.WriteUTF((string)this.content);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PopupWarningMessage(arg1);
		}
		
		public void deserializeAs_PopupWarningMessage(BigEndianReader arg1)
		{
			this.lockDuration = (uint)arg1.ReadByte();
			if ( this.lockDuration < 0 || this.lockDuration > 255 )
			{
				throw new Exception("Forbidden value (" + this.lockDuration + ") on element of PopupWarningMessage.lockDuration.");
			}
			this.author = (String)arg1.ReadUTF();
			this.content = (String)arg1.ReadUTF();
		}
		
	}
}
