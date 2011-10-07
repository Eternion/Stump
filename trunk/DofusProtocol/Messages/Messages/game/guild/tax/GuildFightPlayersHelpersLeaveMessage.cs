// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildFightPlayersHelpersLeaveMessage.xml' the '03/10/2011 12:47:04'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildFightPlayersHelpersLeaveMessage : Message
	{
		public const uint Id = 5719;
		public override uint MessageId
		{
			get
			{
				return 5719;
			}
		}
		
		public double fightId;
		public int playerId;
		
		public GuildFightPlayersHelpersLeaveMessage()
		{
		}
		
		public GuildFightPlayersHelpersLeaveMessage(double fightId, int playerId)
		{
			this.fightId = fightId;
			this.playerId = playerId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteDouble(fightId);
			writer.WriteInt(playerId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			fightId = reader.ReadDouble();
			if ( fightId < 0 )
			{
				throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
			}
			playerId = reader.ReadInt();
			if ( playerId < 0 )
			{
				throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
			}
		}
	}
}
