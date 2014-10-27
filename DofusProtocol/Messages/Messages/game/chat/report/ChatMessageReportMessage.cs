

// Generated on 10/27/2014 19:57:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatMessageReportMessage : Message
    {
        public const uint Id = 821;
        public override uint MessageId
        {
            get { return Id; }
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
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            channel = reader.ReadSByte();
            if (channel < 0)
                throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
            fingerprint = reader.ReadUTF();
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(senderName) + sizeof(short) + Encoding.UTF8.GetByteCount(content) + sizeof(int) + sizeof(sbyte) + sizeof(short) + Encoding.UTF8.GetByteCount(fingerprint) + sizeof(sbyte);
        }
        
    }
    
}