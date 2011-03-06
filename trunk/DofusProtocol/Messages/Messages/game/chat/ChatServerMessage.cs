using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatServerMessage : ChatAbstractServerMessage
	{
		public const uint protocolId = 881;
		internal Boolean _isInitialized = false;
		public int senderId = 0;
		public String senderName = "";
		public int senderAccountId = 0;
		
		public ChatServerMessage()
		{
		}
		
		public ChatServerMessage(uint arg1, String arg2, uint arg3, String arg4, int arg5, String arg6, int arg7)
			: this()
		{
			initChatServerMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 881;
		}
		
		public ChatServerMessage initChatServerMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "", int arg5 = 0, String arg6 = "", int arg7 = 0)
		{
			base.initChatAbstractServerMessage(arg1, arg2, arg3, arg4);
			this.senderId = arg5;
			this.senderName = arg6;
			this.senderAccountId = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.senderId = 0;
			this.senderName = "";
			this.senderAccountId = 0;
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
			this.serializeAs_ChatServerMessage(arg1);
		}
		
		public void serializeAs_ChatServerMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatAbstractServerMessage(arg1);
			arg1.WriteInt((int)this.senderId);
			arg1.WriteUTF((string)this.senderName);
			arg1.WriteInt((int)this.senderAccountId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatServerMessage(arg1);
		}
		
		public void deserializeAs_ChatServerMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.senderId = (int)arg1.ReadInt();
			this.senderName = (String)arg1.ReadUTF();
			this.senderAccountId = (int)arg1.ReadInt();
		}
		
	}
}
