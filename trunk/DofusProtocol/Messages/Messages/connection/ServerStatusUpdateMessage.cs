// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ServerStatusUpdateMessage.xml' the '03/10/2011 12:46:52'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ServerStatusUpdateMessage : Message
	{
		public const uint Id = 50;
		public override uint MessageId
		{
			get
			{
				return 50;
			}
		}
		
		public Types.GameServerInformations server;
		
		public ServerStatusUpdateMessage()
		{
		}
		
		public ServerStatusUpdateMessage(Types.GameServerInformations server)
		{
			this.server = server;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			server.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			server = new Types.GameServerInformations();
			server.Deserialize(reader);
		}
	}
}
