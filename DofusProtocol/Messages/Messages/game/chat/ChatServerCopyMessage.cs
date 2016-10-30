

// Generated on 10/30/2016 16:20:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatServerCopyMessage : ChatAbstractServerMessage
    {
        public const uint Id = 882;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long receiverId;
        public string receiverName;
        
        public ChatServerCopyMessage()
        {
        }
        
        public ChatServerCopyMessage(sbyte channel, string content, int timestamp, string fingerprint, long receiverId, string receiverName)
         : base(channel, content, timestamp, fingerprint)
        {
            this.receiverId = receiverId;
            this.receiverName = receiverName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(receiverId);
            writer.WriteUTF(receiverName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            receiverId = reader.ReadVarLong();
            if (receiverId < 0 || receiverId > 9007199254740990)
                throw new Exception("Forbidden value on receiverId = " + receiverId + ", it doesn't respect the following condition : receiverId < 0 || receiverId > 9007199254740990");
            receiverName = reader.ReadUTF();
        }
        
    }
    
}