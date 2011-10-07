// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChatServerCopyWithObjectMessage.xml' the '03/10/2011 12:46:55'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Messages
{
	public class ChatServerCopyWithObjectMessage : ChatServerCopyMessage
	{
		public const uint Id = 884;
		public override uint MessageId
		{
			get
			{
				return 884;
			}
		}
		
		public IEnumerable<Types.ObjectItem> objects;
		
		public ChatServerCopyWithObjectMessage()
		{
		}
		
		public ChatServerCopyWithObjectMessage(sbyte channel, string content, int timestamp, string fingerprint, int receiverId, string receiverName, IEnumerable<Types.ObjectItem> objects)
			 : base(channel, content, timestamp, fingerprint, receiverId, receiverName)
		{
			this.objects = objects;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUShort((ushort)objects.Count());
			foreach (var entry in objects)
			{
				entry.Serialize(writer);
			}
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			int limit = reader.ReadUShort();
			objects = new Types.ObjectItem[limit];
			for (int i = 0; i < limit; i++)
			{
				(objects as Types.ObjectItem[])[i] = new Types.ObjectItem();
				(objects as Types.ObjectItem[])[i].Deserialize(reader);
			}
		}
	}
}
