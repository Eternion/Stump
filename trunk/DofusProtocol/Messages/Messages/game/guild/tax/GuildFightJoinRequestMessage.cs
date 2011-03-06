using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightJoinRequestMessage : Message
	{
		public const uint protocolId = 5717;
		internal Boolean _isInitialized = false;
		public int taxCollectorId = 0;
		
		public GuildFightJoinRequestMessage()
		{
		}
		
		public GuildFightJoinRequestMessage(int arg1)
			: this()
		{
			initGuildFightJoinRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5717;
		}
		
		public GuildFightJoinRequestMessage initGuildFightJoinRequestMessage(int arg1 = 0)
		{
			this.taxCollectorId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.taxCollectorId = 0;
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
			this.serializeAs_GuildFightJoinRequestMessage(arg1);
		}
		
		public void serializeAs_GuildFightJoinRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.taxCollectorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightJoinRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildFightJoinRequestMessage(BigEndianReader arg1)
		{
			this.taxCollectorId = (int)arg1.ReadInt();
		}
		
	}
}
