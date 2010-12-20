using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildCreationResultMessage : Message
	{
		public const uint protocolId = 5554;
		internal Boolean _isInitialized = false;
		public uint result = 0;
		
		public GuildCreationResultMessage()
		{
		}
		
		public GuildCreationResultMessage(uint arg1)
			: this()
		{
			initGuildCreationResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5554;
		}
		
		public GuildCreationResultMessage initGuildCreationResultMessage(uint arg1 = 0)
		{
			this.result = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.result = 0;
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
			this.serializeAs_GuildCreationResultMessage(arg1);
		}
		
		public void serializeAs_GuildCreationResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.result);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildCreationResultMessage(arg1);
		}
		
		public void deserializeAs_GuildCreationResultMessage(BigEndianReader arg1)
		{
			this.result = (uint)arg1.ReadByte();
			if ( this.result < 0 )
			{
				throw new Exception("Forbidden value (" + this.result + ") on element of GuildCreationResultMessage.result.");
			}
		}
		
	}
}
