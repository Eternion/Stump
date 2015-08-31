

// Generated on 08/04/2015 13:24:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChatClientPrivateWithObjectMessage : ChatClientPrivateMessage
    {
        public const uint Id = 852;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItem> objects;
        
        public ChatClientPrivateWithObjectMessage()
        {
        }
        
        public ChatClientPrivateWithObjectMessage(string content, string receiver, IEnumerable<Types.ObjectItem> objects)
         : base(content, receiver)
        {
            this.objects = objects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var objects_before = writer.Position;
            var objects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
                 objects_count++;
            }
            var objects_after = writer.Position;
            writer.Seek((int)objects_before);
            writer.WriteUShort((ushort)objects_count);
            writer.Seek((int)objects_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var objects_ = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects_[i] = new Types.ObjectItem();
                 objects_[i].Deserialize(reader);
            }
            objects = objects_;
        }
        
    }
    
}