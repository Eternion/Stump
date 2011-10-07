// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GuildLevelUpMessage.xml' the '03/10/2011 12:47:04'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GuildLevelUpMessage : Message
	{
		public const uint Id = 6062;
		public override uint MessageId
		{
			get
			{
				return 6062;
			}
		}
		
		public byte newLevel;
		
		public GuildLevelUpMessage()
		{
		}
		
		public GuildLevelUpMessage(byte newLevel)
		{
			this.newLevel = newLevel;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteByte(newLevel);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			newLevel = reader.ReadByte();
			if ( newLevel < 2 || newLevel > 200 )
			{
				throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 2 || newLevel > 200");
			}
		}
	}
}
