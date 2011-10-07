// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameFightHumanReadyStateMessage.xml' the '03/10/2011 12:46:56'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameFightHumanReadyStateMessage : Message
	{
		public const uint Id = 740;
		public override uint MessageId
		{
			get
			{
				return 740;
			}
		}
		
		public int characterId;
		public bool isReady;
		
		public GameFightHumanReadyStateMessage()
		{
		}
		
		public GameFightHumanReadyStateMessage(int characterId, bool isReady)
		{
			this.characterId = characterId;
			this.isReady = isReady;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(characterId);
			writer.WriteBoolean(isReady);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			characterId = reader.ReadInt();
			if ( characterId < 0 )
			{
				throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
			}
			isReady = reader.ReadBoolean();
		}
	}
}
