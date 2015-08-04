

// Generated on 08/04/2015 13:25:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StorageObjectsRemoveMessage : Message
    {
        public const uint Id = 6035;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> objectUIDList;
        
        public StorageObjectsRemoveMessage()
        {
        }
        
        public StorageObjectsRemoveMessage(IEnumerable<int> objectUIDList)
        {
            this.objectUIDList = objectUIDList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var objectUIDList_before = writer.Position;
            var objectUIDList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectUIDList)
            {
                 writer.WriteVarInt(entry);
                 objectUIDList_count++;
            }
            var objectUIDList_after = writer.Position;
            writer.Seek((int)objectUIDList_before);
            writer.WriteUShort((ushort)objectUIDList_count);
            writer.Seek((int)objectUIDList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var objectUIDList_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectUIDList_[i] = reader.ReadVarInt();
            }
            objectUIDList = objectUIDList_;
        }
        
    }
    
}