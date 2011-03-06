using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightUpdateTeamMessage : Message
	{
		public const uint protocolId = 5572;
		internal Boolean _isInitialized = false;
		public uint fightId = 0;
		public FightTeamInformations team;
		
		public GameFightUpdateTeamMessage()
		{
			this.team = new FightTeamInformations();
		}
		
		public GameFightUpdateTeamMessage(uint arg1, FightTeamInformations arg2)
			: this()
		{
			initGameFightUpdateTeamMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5572;
		}
		
		public GameFightUpdateTeamMessage initGameFightUpdateTeamMessage(uint arg1 = 0, FightTeamInformations arg2 = null)
		{
			this.fightId = arg1;
			this.team = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.team = new FightTeamInformations();
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
			this.serializeAs_GameFightUpdateTeamMessage(arg1);
		}
		
		public void serializeAs_GameFightUpdateTeamMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteShort((short)this.fightId);
			this.team.serializeAs_FightTeamInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightUpdateTeamMessage(arg1);
		}
		
		public void deserializeAs_GameFightUpdateTeamMessage(BigEndianReader arg1)
		{
			this.fightId = (uint)arg1.ReadShort();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GameFightUpdateTeamMessage.fightId.");
			}
			this.team = new FightTeamInformations();
			this.team.deserialize(arg1);
		}
		
	}
}
