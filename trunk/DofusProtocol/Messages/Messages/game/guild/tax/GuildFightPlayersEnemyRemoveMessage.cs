using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightPlayersEnemyRemoveMessage : Message
	{
		public const uint protocolId = 5929;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint playerId = 0;
		
		public GuildFightPlayersEnemyRemoveMessage()
		{
		}
		
		public GuildFightPlayersEnemyRemoveMessage(double arg1, uint arg2)
			: this()
		{
			initGuildFightPlayersEnemyRemoveMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5929;
		}
		
		public GuildFightPlayersEnemyRemoveMessage initGuildFightPlayersEnemyRemoveMessage(double arg1 = 0, uint arg2 = 0)
		{
			this.fightId = arg1;
			this.playerId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.playerId = 0;
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
			this.serializeAs_GuildFightPlayersEnemyRemoveMessage(arg1);
		}
		
		public void serializeAs_GuildFightPlayersEnemyRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteDouble(this.fightId);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightPlayersEnemyRemoveMessage(arg1);
		}
		
		public void deserializeAs_GuildFightPlayersEnemyRemoveMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GuildFightPlayersEnemyRemoveMessage.fightId.");
			}
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of GuildFightPlayersEnemyRemoveMessage.playerId.");
			}
		}
		
	}
}
