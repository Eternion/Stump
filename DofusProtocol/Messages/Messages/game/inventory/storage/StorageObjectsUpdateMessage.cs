

// Generated on 04/24/2015 03:38:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StorageObjectsUpdateMessage : Message
    {
        public const uint Id = 6036;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItem> objectList;
        
        public StorageObjectsUpdateMessage()
        {
        }
        
        public StorageObjectsUpdateMessage(IEnumerable<Types.ObjectItem> objectList)
        {
            this.objectList = objectList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var objectList_before = writer.Position;
            var objectList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectList)
            {
                 entry.Serialize(writer);
                 objectList_count++;
            }
            var objectList_after = writer.Position;
            writer.Seek((int)objectList_before);
            writer.WriteUShort((ushort)objectList_count);
            writer.Seek((int)objectList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var objectList_ = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectList_[i] = new Types.ObjectItem();
                 objectList_[i].Deserialize(reader);
            }
            objectList = objectList_;
        }
        
    }
    
}