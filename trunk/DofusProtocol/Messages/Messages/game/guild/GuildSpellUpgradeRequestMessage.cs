using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildSpellUpgradeRequestMessage : Message
	{
		public const uint protocolId = 5699;
		internal Boolean _isInitialized = false;
		public uint spellId = 0;
		
		public GuildSpellUpgradeRequestMessage()
		{
		}
		
		public GuildSpellUpgradeRequestMessage(uint arg1)
			: this()
		{
			initGuildSpellUpgradeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5699;
		}
		
		public GuildSpellUpgradeRequestMessage initGuildSpellUpgradeRequestMessage(uint arg1 = 0)
		{
			this.spellId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellId = 0;
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
			this.serializeAs_GuildSpellUpgradeRequestMessage(arg1);
		}
		
		public void serializeAs_GuildSpellUpgradeRequestMessage(BigEndianWriter arg1)
		{
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteInt((int)this.spellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildSpellUpgradeRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildSpellUpgradeRequestMessage(BigEndianReader arg1)
		{
			this.spellId = (uint)arg1.ReadInt();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GuildSpellUpgradeRequestMessage.spellId.");
			}
		}
		
	}
}
