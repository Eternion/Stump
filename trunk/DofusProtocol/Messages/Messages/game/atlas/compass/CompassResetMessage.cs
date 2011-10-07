// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CompassResetMessage.xml' the '03/10/2011 12:46:54'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CompassResetMessage : Message
	{
		public const uint Id = 5584;
		public override uint MessageId
		{
			get
			{
				return 5584;
			}
		}
		
		public sbyte type;
		
		public CompassResetMessage()
		{
		}
		
		public CompassResetMessage(sbyte type)
		{
			this.type = type;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(type);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			type = reader.ReadSByte();
			if ( type < 0 )
			{
				throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
			}
		}
	}
}
