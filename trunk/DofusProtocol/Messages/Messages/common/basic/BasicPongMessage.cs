// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'BasicPongMessage.xml' the '03/10/2011 12:46:52'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class BasicPongMessage : Message
	{
		public const uint Id = 183;
		public override uint MessageId
		{
			get
			{
				return 183;
			}
		}
		
		public bool quiet;
		
		public BasicPongMessage()
		{
		}
		
		public BasicPongMessage(bool quiet)
		{
			this.quiet = quiet;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(quiet);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			quiet = reader.ReadBoolean();
		}
	}
}
