using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightPlayersHelpersJoinMessage : Message
	{
		public const uint protocolId = 5720;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public CharacterMinimalPlusLookInformations playerInfo;
		
		public GuildFightPlayersHelpersJoinMessage()
		{
			this.playerInfo = new CharacterMinimalPlusLookInformations();
		}
		
		public GuildFightPlayersHelpersJoinMessage(double arg1, CharacterMinimalPlusLookInformations arg2)
			: this()
		{
			initGuildFightPlayersHelpersJoinMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5720;
		}
		
		public GuildFightPlayersHelpersJoinMessage initGuildFightPlayersHelpersJoinMessage(double arg1 = 0, CharacterMinimalPlusLookInformations arg2 = null)
		{
			this.fightId = arg1;
			this.playerInfo = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.playerInfo = new CharacterMinimalPlusLookInformations();
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
			this.serializeAs_GuildFightPlayersHelpersJoinMessage(arg1);
		}
		
		public void serializeAs_GuildFightPlayersHelpersJoinMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteDouble(this.fightId);
			this.playerInfo.serializeAs_CharacterMinimalPlusLookInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightPlayersHelpersJoinMessage(arg1);
		}
		
		public void deserializeAs_GuildFightPlayersHelpersJoinMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GuildFightPlayersHelpersJoinMessage.fightId.");
			}
			this.playerInfo = new CharacterMinimalPlusLookInformations();
			this.playerInfo.deserialize(arg1);
		}
		
	}
}
