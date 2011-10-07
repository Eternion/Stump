// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildHousesInformationMessage.xml' the '03/10/2011 12:47:03'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class GuildHousesInformationMessage : Message
	{
		public const uint Id = 5919;
		public override uint MessageId
		{
			get
			{
				return 5919;
			}
		}
		
		public IEnumerable<Types.HouseInformationsForGuild> housesInformations;
		
		public GuildHousesInformationMessage()
		{
		}
		
		public GuildHousesInformationMessage(IEnumerable<Types.HouseInformationsForGuild> housesInformations)
		{
			this.housesInformations = housesInformations;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUShort((ushort)housesInformations.Count());
			foreach (var entry in housesInformations)
			{
				entry.Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			int limit = reader.ReadUShort();
			housesInformations = new Types.HouseInformationsForGuild[limit];
			for (int i = 0; i < limit; i++)
			{
				(housesInformations as Types.HouseInformationsForGuild[])[i] = new Types.HouseInformationsForGuild();
				(housesInformations as Types.HouseInformationsForGuild[])[i].Deserialize(reader);
			}
		}
	}
}
