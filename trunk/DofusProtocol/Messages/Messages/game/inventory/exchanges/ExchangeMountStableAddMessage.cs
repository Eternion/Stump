// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeMountStableAddMessage.xml' the '03/10/2011 12:47:06'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeMountStableAddMessage : Message
	{
		public const uint Id = 5971;
		public override uint MessageId
		{
			get
			{
				return 5971;
			}
		}
		
		public Types.MountClientData mountDescription;
		
		public ExchangeMountStableAddMessage()
		{
		}
		
		public ExchangeMountStableAddMessage(Types.MountClientData mountDescription)
		{
			this.mountDescription = mountDescription;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			mountDescription.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			mountDescription = new Types.MountClientData();
			mountDescription.Deserialize(reader);
		}
	}
}
