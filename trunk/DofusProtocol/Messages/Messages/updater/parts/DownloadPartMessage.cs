// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'DownloadPartMessage.xml' the '03/10/2011 12:47:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class DownloadPartMessage : Message
	{
		public const uint Id = 1503;
		public override uint MessageId
		{
			get
			{
				return 1503;
			}
		}
		
		public string id;
		
		public DownloadPartMessage()
		{
		}
		
		public DownloadPartMessage(string id)
		{
			this.id = id;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(id);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			id = reader.ReadUTF();
		}
	}
}
