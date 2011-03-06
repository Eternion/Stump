using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatAbstractClientMessage : Message
	{
		public const uint protocolId = 850;
		internal Boolean _isInitialized = false;
		public String content = "";
		
		public ChatAbstractClientMessage()
		{
		}
		
		public ChatAbstractClientMessage(String arg1)
			: this()
		{
			initChatAbstractClientMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 850;
		}
		
		public ChatAbstractClientMessage initChatAbstractClientMessage(String arg1 = "")
		{
			this.content = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
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
			this.serializeAs_ChatAbstractClientMessage(arg1);
		}
		
		public void serializeAs_ChatAbstractClientMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.content);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatAbstractClientMessage(arg1);
		}
		
		public void deserializeAs_ChatAbstractClientMessage(BigEndianReader arg1)
		{
			this.content = (String)arg1.ReadUTF();
		}
		
	}
}
