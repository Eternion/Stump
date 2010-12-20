using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightLeaveRequestMessage : Message
	{
		public const uint protocolId = 5715;
		internal Boolean _isInitialized = false;
		public int taxCollectorId = 0;
		public uint characterId = 0;
		
		public GuildFightLeaveRequestMessage()
		{
		}
		
		public GuildFightLeaveRequestMessage(int arg1, uint arg2)
			: this()
		{
			initGuildFightLeaveRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5715;
		}
		
		public GuildFightLeaveRequestMessage initGuildFightLeaveRequestMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.taxCollectorId = arg1;
			this.characterId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.taxCollectorId = 0;
			this.characterId = 0;
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
			this.serializeAs_GuildFightLeaveRequestMessage(arg1);
		}
		
		public void serializeAs_GuildFightLeaveRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.taxCollectorId);
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element characterId.");
			}
			arg1.WriteInt((int)this.characterId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightLeaveRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildFightLeaveRequestMessage(BigEndianReader arg1)
		{
			this.taxCollectorId = (int)arg1.ReadInt();
			this.characterId = (uint)arg1.ReadInt();
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element of GuildFightLeaveRequestMessage.characterId.");
			}
		}
		
	}
}
