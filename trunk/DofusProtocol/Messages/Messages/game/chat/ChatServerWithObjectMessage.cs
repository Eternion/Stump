

// Generated on 07/29/2013 23:07:44
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatServerWithObjectMessage : ChatServerMessage
    {
        public const uint Id = 883;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItem> objects;
        
        public ChatServerWithObjectMessage()
        {
        }
        
        public ChatServerWithObjectMessage(sbyte channel, string content, int timestamp, string fingerprint, int senderId, string senderName, int senderAccountId, IEnumerable<Types.ObjectItem> objects)
         : base(channel, content, timestamp, fingerprint, senderId, senderName, senderAccountId)
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
            var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objects as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objects as Types.ObjectItem[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + objects.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}