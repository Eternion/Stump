// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeObjectTransfertAllToInvMessage.xml' the '03/10/2011 12:47:06'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeObjectTransfertAllToInvMessage : Message
	{
		public const uint Id = 6032;
		public override uint MessageId
		{
			get
			{
				return 6032;
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
