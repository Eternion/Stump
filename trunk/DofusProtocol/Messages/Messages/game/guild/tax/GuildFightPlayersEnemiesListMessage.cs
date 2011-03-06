using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightPlayersEnemiesListMessage : Message
	{
		public const uint protocolId = 5928;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public List<CharacterMinimalPlusLookInformations> playerInfo;
		
		public GuildFightPlayersEnemiesListMessage()
		{
			this.playerInfo = new List<CharacterMinimalPlusLookInformations>();
		}
		
		public GuildFightPlayersEnemiesListMessage(double arg1, List<CharacterMinimalPlusLookInformations> arg2)
			: this()
		{
			initGuildFightPlayersEnemiesListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5928;
		}
		
		public GuildFightPlayersEnemiesListMessage initGuildFightPlayersEnemiesListMessage(double arg1 = 0, List<CharacterMinimalPlusLookInformations> arg2 = null)
		{
			this.fightId = arg1;
			this.playerInfo = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.playerInfo = new List<CharacterMinimalPlusLookInformations>();
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
			this.serializeAs_GuildFightPlayersEnemiesListMessage(arg1);
		}
		
		public void serializeAs_GuildFightPlayersEnemiesListMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteDouble(this.fightId);
			arg1.WriteShort((short)this.playerInfo.Count);
			var loc1 = 0;
			while ( loc1 < this.playerInfo.Count )
			{
				this.playerInfo[loc1].serializeAs_CharacterMinimalPlusLookInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightPlayersEnemiesListMessage(arg1);
		}
		
		public void deserializeAs_GuildFightPlayersEnemiesListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.fightId = (double)arg1.ReadDouble();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GuildFightPlayersEnemiesListMessage.fightId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new CharacterMinimalPlusLookInformations()) as CharacterMinimalPlusLookInformations).deserialize(arg1);
				this.playerInfo.Add((CharacterMinimalPlusLookInformations)loc3);
				++loc2;
			}
		}
		
	}
}
