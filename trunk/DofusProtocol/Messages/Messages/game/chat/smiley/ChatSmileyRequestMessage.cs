using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatSmileyRequestMessage : Message
	{
		public const uint protocolId = 800;
		internal Boolean _isInitialized = false;
		public uint smileyId = 0;
		
		public ChatSmileyRequestMessage()
		{
		}
		
		public ChatSmileyRequestMessage(uint arg1)
			: this()
		{
			initChatSmileyRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 800;
		}
		
		public ChatSmileyRequestMessage initChatSmileyRequestMessage(uint arg1 = 0)
		{
			this.smileyId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.smileyId = 0;
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
			this.serializeAs_ChatSmileyRequestMessage(arg1);
		}
		
		public void serializeAs_ChatSmileyRequestMessage(BigEndianWriter arg1)
		{
			if ( this.smileyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.smileyId + ") on element smileyId.");
			}
			arg1.WriteByte((byte)this.smileyId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatSmileyRequestMessage(arg1);
		}
		
		public void deserializeAs_ChatSmileyRequestMessage(BigEndianReader arg1)
		{
			this.smileyId = (uint)arg1.ReadByte();
			if ( this.smileyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.smileyId + ") on element of ChatSmileyRequestMessage.smileyId.");
			}
		}
		
	}
}
