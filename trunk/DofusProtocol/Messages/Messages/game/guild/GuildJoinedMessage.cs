// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildJoinedMessage.xml' the '03/10/2011 12:47:04'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildJoinedMessage : Message
	{
		public const uint Id = 5564;
		public override uint MessageId
		{
			get
			{
				return 5564;
			}
		}
		
		public Types.GuildInformations guildInfo;
		public uint memberRights;
		
		public GuildJoinedMessage()
		{
		}
		
		public GuildJoinedMessage(Types.GuildInformations guildInfo, uint memberRights)
		{
			this.guildInfo = guildInfo;
			this.memberRights = memberRights;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			guildInfo.Serialize(writer);
			writer.WriteUInt(memberRights);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			guildInfo = new Types.GuildInformations();
			guildInfo.Deserialize(reader);
			memberRights = reader.ReadUInt();
			if ( memberRights < 0 || memberRights > 4294967295 )
			{
				throw new Exception("Forbidden value on memberRights = " + memberRights + ", it doesn't respect the following condition : memberRights < 0 || memberRights > 4294967295");
			}
		}
	}
}
