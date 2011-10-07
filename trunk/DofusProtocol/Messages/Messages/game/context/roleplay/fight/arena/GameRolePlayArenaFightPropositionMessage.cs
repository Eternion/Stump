// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlayArenaFightPropositionMessage.xml' the '03/10/2011 12:46:58'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class GameRolePlayArenaFightPropositionMessage : Message
	{
		public const uint Id = 6276;
		public override uint MessageId
		{
			get
			{
				return 6276;
			}
		}
		
		public int fightId;
		public IEnumerable<int> alliesId;
		public short duration;
		
		public GameRolePlayArenaFightPropositionMessage()
		{
		}
		
		public GameRolePlayArenaFightPropositionMessage(int fightId, IEnumerable<int> alliesId, short duration)
		{
			this.fightId = fightId;
			this.alliesId = alliesId;
			this.duration = duration;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(fightId);
			writer.WriteUShort((ushort)alliesId.Count());
			foreach (var entry in alliesId)
			{
				writer.WriteInt(entry);
			}
			writer.WriteShort(duration);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			fightId = reader.ReadInt();
			if ( fightId < 0 )
			{
				throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
			}
			int limit = reader.ReadUShort();
			alliesId = new int[limit];
			for (int i = 0; i < limit; i++)
			{
				(alliesId as int[])[i] = reader.ReadInt();
			}
			duration = reader.ReadShort();
			if ( duration < 0 )
			{
				throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
			}
		}
	}
}
