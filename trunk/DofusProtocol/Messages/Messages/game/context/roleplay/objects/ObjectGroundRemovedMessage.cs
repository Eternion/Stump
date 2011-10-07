// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectGroundRemovedMessage.xml' the '03/10/2011 12:47:00'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectGroundRemovedMessage : Message
	{
		public const uint Id = 3014;
		public override uint MessageId
		{
			get
			{
				return 3014;
			}
		}
		
		public short cell;
		
		public ObjectGroundRemovedMessage()
		{
		}
		
		public ObjectGroundRemovedMessage(short cell)
		{
			this.cell = cell;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(cell);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			cell = reader.ReadShort();
			if ( cell < 0 || cell > 559 )
			{
				throw new Exception("Forbidden value on cell = " + cell + ", it doesn't respect the following condition : cell < 0 || cell > 559");
			}
		}
	}
}
