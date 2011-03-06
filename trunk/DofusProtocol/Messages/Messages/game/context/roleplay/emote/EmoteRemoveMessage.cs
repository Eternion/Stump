using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class EmoteRemoveMessage : Message
	{
		public const uint protocolId = 5687;
		internal Boolean _isInitialized = false;
		public uint emoteId = 0;
		
		public EmoteRemoveMessage()
		{
		}
		
		public EmoteRemoveMessage(uint arg1)
			: this()
		{
			initEmoteRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5687;
		}
		
		public EmoteRemoveMessage initEmoteRemoveMessage(uint arg1 = 0)
		{
			this.emoteId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.emoteId = 0;
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
			this.serializeAs_EmoteRemoveMessage(arg1);
		}
		
		public void serializeAs_EmoteRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.emoteId < 0 )
			{
				throw new Exception("Forbidden value (" + this.emoteId + ") on element emoteId.");
			}
			arg1.WriteByte((byte)this.emoteId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EmoteRemoveMessage(arg1);
		}
		
		public void deserializeAs_EmoteRemoveMessage(BigEndianReader arg1)
		{
			this.emoteId = (uint)arg1.ReadByte();
			if ( this.emoteId < 0 )
			{
				throw new Exception("Forbidden value (" + this.emoteId + ") on element of EmoteRemoveMessage.emoteId.");
			}
		}
		
	}
}
