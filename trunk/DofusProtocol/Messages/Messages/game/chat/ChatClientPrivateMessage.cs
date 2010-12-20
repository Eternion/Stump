using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatClientPrivateMessage : ChatAbstractClientMessage
	{
		public const uint protocolId = 851;
		internal Boolean _isInitialized = false;
		public String receiver = "";
		
		public ChatClientPrivateMessage()
		{
		}
		
		public ChatClientPrivateMessage(String arg1, String arg2)
			: this()
		{
			initChatClientPrivateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 851;
		}
		
		public ChatClientPrivateMessage initChatClientPrivateMessage(String arg1 = "", String arg2 = "")
		{
			base.initChatAbstractClientMessage(arg1);
			this.receiver = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.receiver = "";
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
			this.serializeAs_ChatClientPrivateMessage(arg1);
		}
		
		public void serializeAs_ChatClientPrivateMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatAbstractClientMessage(arg1);
			arg1.WriteUTF((string)this.receiver);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatClientPrivateMessage(arg1);
		}
		
		public void deserializeAs_ChatClientPrivateMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.receiver = (String)arg1.ReadUTF();
		}
		
	}
}
