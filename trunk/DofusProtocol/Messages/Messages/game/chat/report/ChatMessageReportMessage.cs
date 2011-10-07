// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ChatMessageReportMessage.xml' the '03/10/2011 12:46:56'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ChatMessageReportMessage : Message
	{
		public const uint Id = 821;
		public override uint MessageId
		{
			get
			{
				return 821;
			}
		}
		
		public string senderName;
		public string content;
		public int timestamp;
		public sbyte channel;
		public string fingerprint;
		public sbyte reason;
		
		public ChatMessageReportMessage()
		{
		}
		
		public ChatMessageReportMessage(string senderName, string content, int timestamp, sbyte channel, string fingerprint, sbyte reason)
		{
			this.senderName = senderName;
			this.content = content;
			this.timestamp = timestamp;
			this.channel = channel;
			this.fingerprint = fingerprint;
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteUTF(senderName);
			writer.WriteUTF(content);
			writer.WriteInt(timestamp);
			writer.WriteSByte(channel);
			writer.WriteUTF(fingerprint);
			writer.WriteSByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			senderName = reader.ReadUTF();
			content = reader.ReadUTF();
			timestamp = reader.ReadInt();
			if ( timestamp < 0 )
			{
				throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
			}
			channel = reader.ReadSByte();
			if ( channel < 0 )
			{
				throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
			}
			fingerprint = reader.ReadUTF();
			reason = reader.ReadSByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}
