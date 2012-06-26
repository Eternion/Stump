// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'AlternativeMonstersInGroupLightInformations.xml' the '26/06/2012 18:48:02'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class AlternativeMonstersInGroupLightInformations
	{
		public const uint Id = 394;
		public virtual short TypeId
		{
			get
			{
				return 394;
			}
		}
		
		public int playerCount;
		public IEnumerable<Types.MonsterInGroupLightInformations> monsters;
		
		public AlternativeMonstersInGroupLightInformations()
		{
		}
		
		public AlternativeMonstersInGroupLightInformations(int playerCount, IEnumerable<Types.MonsterInGroupLightInformations> monsters)
		{
			this.playerCount = playerCount;
			this.monsters = monsters;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(playerCount);
			writer.WriteUShort((ushort)monsters.Count());
			foreach (var entry in monsters)
			{
				entry.Serialize(writer);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			playerCount = reader.ReadInt();
			int limit = reader.ReadUShort();
			monsters = new Types.MonsterInGroupLightInformations[limit];
			for (int i = 0; i < limit; i++)
			{
				(monsters as MonsterInGroupLightInformations[])[i] = new Types.MonsterInGroupLightInformations();
				(monsters as Types.MonsterInGroupLightInformations[])[i].Deserialize(reader);
			}
		}
	}
}