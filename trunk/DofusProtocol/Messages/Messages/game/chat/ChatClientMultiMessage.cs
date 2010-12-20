using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatClientMultiMessage : ChatAbstractClientMessage
	{
		public const uint protocolId = 861;
		internal Boolean _isInitialized = false;
		public uint channel = 0;
		
		public ChatClientMultiMessage()
		{
		}
		
		public ChatClientMultiMessage(String arg1, uint arg2)
			: this()
		{
			initChatClientMultiMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 861;
		}
		
		public ChatClientMultiMessage initChatClientMultiMessage(String arg1 = "", uint arg2 = 0)
		{
			base.initChatAbstractClientMessage(arg1);
			this.channel = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.channel = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ChatClientMultiMessage(arg1);
		}
		
		public void serializeAs_ChatClientMultiMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatAbstractClientMessage(arg1);
			arg1.WriteByte((byte)this.channel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatClientMultiMessage(arg1);
		}
		
		public void deserializeAs_ChatClientMultiMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.channel = (uint)arg1.ReadByte();
			if ( this.channel < 0 )
			{
				throw new Exception("Forbidden value (" + this.channel + ") on element of ChatClientMultiMessage.channel.");
			}
		}
		
	}
}
