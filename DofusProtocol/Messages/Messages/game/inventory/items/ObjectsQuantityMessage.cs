

// Generated on 04/19/2016 10:17:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectsQuantityMessage : Message
    {
        public const uint Id = 6206;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty;
        
        public ObjectsQuantityMessage()
        {
        }
        
        public ObjectsQuantityMessage(IEnumerable<Types.ObjectItemQuantity> objectsUIDAndQty)
        {
            this.objectsUIDAndQty = objectsUIDAndQty;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var objectsUIDAndQty_before = writer.Position;
            var objectsUIDAndQty_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectsUIDAndQty)
            {
                 entry.Serialize(writer);
                 objectsUIDAndQty_count++;
            }
            var objectsUIDAndQty_after = writer.Position;
            writer.Seek((int)objectsUIDAndQty_before);
            writer.WriteUShort((ushort)objectsUIDAndQty_count);
            writer.Seek((int)objectsUIDAndQty_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var objectsUIDAndQty_ = new Types.ObjectItemQuantity[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsUIDAndQty_[i] = new Types.ObjectItemQuantity();
                 objectsUIDAndQty_[i].Deserialize(reader);
            }
            objectsUIDAndQty = objectsUIDAndQty_;
        }
        
    }
    
}