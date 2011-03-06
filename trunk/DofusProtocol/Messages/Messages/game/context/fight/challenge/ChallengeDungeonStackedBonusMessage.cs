using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeDungeonStackedBonusMessage : Message
	{
		public const uint protocolId = 6151;
		internal Boolean _isInitialized = false;
		public uint dungeonId = 0;
		public uint xpBonus = 0;
		public uint dropBonus = 0;
		
		public ChallengeDungeonStackedBonusMessage()
		{
		}
		
		public ChallengeDungeonStackedBonusMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initChallengeDungeonStackedBonusMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6151;
		}
		
		public ChallengeDungeonStackedBonusMessage initChallengeDungeonStackedBonusMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.dungeonId = arg1;
			this.xpBonus = arg2;
			this.dropBonus = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.dungeonId = 0;
			this.xpBonus = 0;
			this.dropBonus = 0;
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
			this.serializeAs_ChallengeDungeonStackedBonusMessage(arg1);
		}
		
		public void serializeAs_ChallengeDungeonStackedBonusMessage(BigEndianWriter arg1)
		{
			if ( this.dungeonId < 0 )
			{
				throw new Exception("Forbidden value (" + this.dungeonId + ") on element dungeonId.");
			}
			arg1.WriteInt((int)this.dungeonId);
			if ( this.xpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.xpBonus + ") on element xpBonus.");
			}
			arg1.WriteInt((int)this.xpBonus);
			if ( this.dropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.dropBonus + ") on element dropBonus.");
			}
			arg1.WriteInt((int)this.dropBonus);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeDungeonStackedBonusMessage(arg1);
		}
		
		public void deserializeAs_ChallengeDungeonStackedBonusMessage(BigEndianReader arg1)
		{
			this.dungeonId = (uint)arg1.ReadInt();
			if ( this.dungeonId < 0 )
			{
				throw new Exception("Forbidden value (" + this.dungeonId + ") on element of ChallengeDungeonStackedBonusMessage.dungeonId.");
			}
			this.xpBonus = (uint)arg1.ReadInt();
			if ( this.xpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.xpBonus + ") on element of ChallengeDungeonStackedBonusMessage.xpBonus.");
			}
			this.dropBonus = (uint)arg1.ReadInt();
			if ( this.dropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.dropBonus + ") on element of ChallengeDungeonStackedBonusMessage.dropBonus.");
			}
		}
		
	}
}
