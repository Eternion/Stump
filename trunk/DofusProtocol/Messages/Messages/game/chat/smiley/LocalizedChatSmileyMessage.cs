using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LocalizedChatSmileyMessage : ChatSmileyMessage
	{
		public const uint protocolId = 6185;
		internal Boolean _isInitialized = false;
		public uint cellId = 0;
		
		public LocalizedChatSmileyMessage()
		{
		}
		
		public LocalizedChatSmileyMessage(int arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initLocalizedChatSmileyMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6185;
		}
		
		public LocalizedChatSmileyMessage initLocalizedChatSmileyMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initChatSmileyMessage(arg1, arg2, arg3);
			this.cellId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.cellId = 0;
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
			this.serializeAs_LocalizedChatSmileyMessage(arg1);
		}
		
		public void serializeAs_LocalizedChatSmileyMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatSmileyMessage(arg1);
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LocalizedChatSmileyMessage(arg1);
		}
		
		public void deserializeAs_LocalizedChatSmileyMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of LocalizedChatSmileyMessage.cellId.");
			}
		}
		
	}
}
