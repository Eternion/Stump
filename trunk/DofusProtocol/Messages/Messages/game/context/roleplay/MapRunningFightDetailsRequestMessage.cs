// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'MapRunningFightDetailsRequestMessage.xml' the '03/10/2011 12:46:58'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class MapRunningFightDetailsRequestMessage : Message
	{
		public const uint Id = 5750;
		public override uint MessageId
		{
			get
			{
				return 5750;
			}
		}
		
		public int fightId;
		
		public MapRunningFightDetailsRequestMessage()
		{
		}
		
		public MapRunningFightDetailsRequestMessage(int fightId)
		{
			this.fightId = fightId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(fightId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			fightId = reader.ReadInt();
			if ( fightId < 0 )
			{
				throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
			}
		}
	}
}
