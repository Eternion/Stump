// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChatClientPrivateMessage.xml' the '03/10/2011 12:46:55'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ChatClientPrivateMessage : ChatAbstractClientMessage
	{
		public const uint Id = 851;
		public override uint MessageId
		{
			get
			{
				return 851;
			}
		}
		
		public string receiver;
		
		public ChatClientPrivateMessage()
		{
		}
		
		public ChatClientPrivateMessage(string content, string receiver)
			 : base(content)
		{
			this.receiver = receiver;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteUTF(receiver);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			receiver = reader.ReadUTF();
		}
	}
}
