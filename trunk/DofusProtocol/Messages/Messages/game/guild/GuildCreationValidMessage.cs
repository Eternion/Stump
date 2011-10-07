// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildCreationValidMessage.xml' the '03/10/2011 12:47:03'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildCreationValidMessage : Message
	{
		public const uint Id = 5546;
		public override uint MessageId
		{
			get
			{
				return 5546;
			}
		}
		
		public string guildName;
		public Types.GuildEmblem guildEmblem;
		
		public GuildCreationValidMessage()
		{
		}
		
		public GuildCreationValidMessage(string guildName, Types.GuildEmblem guildEmblem)
		{
			this.guildName = guildName;
			this.guildEmblem = guildEmblem;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(guildName);
			guildEmblem.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			guildName = reader.ReadUTF();
			guildEmblem = new Types.GuildEmblem();
			guildEmblem.Deserialize(reader);
		}
	}
}
