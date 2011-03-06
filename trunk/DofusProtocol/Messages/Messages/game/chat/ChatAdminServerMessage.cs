using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatAdminServerMessage : ChatServerMessage
	{
		public const uint protocolId = 6135;
		internal Boolean _isInitialized = false;
		
		public ChatAdminServerMessage()
		{
		}
		
		public ChatAdminServerMessage(uint arg1, String arg2, uint arg3, String arg4, int arg5, String arg6, int arg7)
			: this()
		{
			initChatAdminServerMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 6135;
		}
		
		public ChatAdminServerMessage initChatAdminServerMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "", int arg5 = 0, String arg6 = "", int arg7 = 0)
		{
			base.initChatServerMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_ChatAdminServerMessage(arg1);
		}
		
		public void serializeAs_ChatAdminServerMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatServerMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatAdminServerMessage(arg1);
		}
		
		public void deserializeAs_ChatAdminServerMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
