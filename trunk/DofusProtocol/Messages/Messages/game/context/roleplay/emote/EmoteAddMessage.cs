using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class EmoteAddMessage : Message
	{
		public const uint protocolId = 5644;
		internal Boolean _isInitialized = false;
		public uint emoteId = 0;
		
		public EmoteAddMessage()
		{
		}
		
		public EmoteAddMessage(uint arg1)
			: this()
		{
			initEmoteAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5644;
		}
		
		public EmoteAddMessage initEmoteAddMessage(uint arg1 = 0)
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
			this.serializeAs_EmoteAddMessage(arg1);
		}
		
		public void serializeAs_EmoteAddMessage(BigEndianWriter arg1)
		{
			if ( this.emoteId < 0 )
			{
				throw new Exception("Forbidden value (" + this.emoteId + ") on element emoteId.");
			}
			arg1.WriteByte((byte)this.emoteId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EmoteAddMessage(arg1);
		}
		
		public void deserializeAs_EmoteAddMessage(BigEndianReader arg1)
		{
			this.emoteId = (uint)arg1.ReadByte();
			if ( this.emoteId < 0 )
			{
				throw new Exception("Forbidden value (" + this.emoteId + ") on element of EmoteAddMessage.emoteId.");
			}
		}
		
	}
}
