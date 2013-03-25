
// Generated on 03/25/2013 19:24:23
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)objectList.Count());
            foreach (var entry in objectList)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objectList = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectList as Types.ObjectItem[])[i] = new Types.ObjectItem();
                 (objectList as Types.ObjectItem[])[i].Deserialize(reader);
            }
        }
        
    }
    
}