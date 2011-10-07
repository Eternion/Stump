// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameMapChangeOrientationRequestMessage.xml' the '03/10/2011 12:46:56'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameMapChangeOrientationRequestMessage : Message
	{
		public const uint Id = 945;
		public override uint MessageId
		{
			get
			{
				return 945;
			}
		}
		
		public sbyte direction;
		
		public GameMapChangeOrientationRequestMessage()
		{
		}
		
		public GameMapChangeOrientationRequestMessage(sbyte direction)
		{
			this.direction = direction;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(direction);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			direction = reader.ReadSByte();
			if ( direction < 0 )
			{
				throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
			}
		}
	}
}
