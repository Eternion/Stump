// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'HelloGameMessage.xml' the '03/10/2011 12:46:54'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class HelloGameMessage : Message
	{
		public const uint Id = 101;
		public override uint MessageId
		{
			get
			{
				return 101;
			}
		}
		
		
		public override void Serialize(IDataWriter writer)
		{
		}
		
		public override void Deserialize(IDataReader reader)
		{
		}
	}
}
