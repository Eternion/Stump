
// Generated on 03/25/2013 19:24:05
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatServerCopyWithObjectMessage : ChatServerCopyMessage
    {
        public const uint Id = 884;
        public override uint MessageId
        {
            get { return Id; }
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
            var limit = reader.ReadUShort();
            objects = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objects as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objects as Types.ObjectItem[])[i].Deserialize(reader);
            }
        }
        
    }
    
}