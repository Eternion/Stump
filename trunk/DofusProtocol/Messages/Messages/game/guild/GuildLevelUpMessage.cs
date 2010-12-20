using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildLevelUpMessage : Message
	{
		public const uint protocolId = 6062;
		internal Boolean _isInitialized = false;
		public uint newLevel = 0;
		
		public GuildLevelUpMessage()
		{
		}
		
		public GuildLevelUpMessage(uint arg1)
			: this()
		{
			initGuildLevelUpMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6062;
		}
		
		public GuildLevelUpMessage initGuildLevelUpMessage(uint arg1 = 0)
		{
			this.newLevel = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.newLevel = 0;
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
			this.serializeAs_GuildLevelUpMessage(arg1);
		}
		
		public void serializeAs_GuildLevelUpMessage(BigEndianWriter arg1)
		{
			if ( this.newLevel < 2 || this.newLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element newLevel.");
			}
			arg1.WriteByte((byte)this.newLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildLevelUpMessage(arg1);
		}
		
		public void deserializeAs_GuildLevelUpMessage(BigEndianReader arg1)
		{
			this.newLevel = (uint)arg1.ReadByte();
			if ( this.newLevel < 2 || this.newLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element of GuildLevelUpMessage.newLevel.");
			}
		}
		
	}
}
