

// Generated on 10/30/2016 16:20:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemGenericQuantity : Item
    {
        public const short Id = 483;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short objectGID;
        public int quantity;
        
        public ObjectItemGenericQuantity()
        {
        }
        
        public ObjectItemGenericQuantity(short objectGID, int quantity)
        {
            this.objectGID = objectGID;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(objectGID);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGID = reader.ReadVarShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
        
    }
    
}