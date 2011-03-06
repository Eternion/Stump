using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildUIOpenedMessage : Message
	{
		public const uint protocolId = 5561;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		
		public GuildUIOpenedMessage()
		{
		}
		
		public GuildUIOpenedMessage(uint arg1)
			: this()
		{
			initGuildUIOpenedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5561;
		}
		
		public GuildUIOpenedMessage initGuildUIOpenedMessage(uint arg1 = 0)
		{
			this.type = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
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
			this.serializeAs_GuildUIOpenedMessage(arg1);
		}
		
		public void serializeAs_GuildUIOpenedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildUIOpenedMessage(arg1);
		}
		
		public void deserializeAs_GuildUIOpenedMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of GuildUIOpenedMessage.type.");
			}
		}
		
	}
}
