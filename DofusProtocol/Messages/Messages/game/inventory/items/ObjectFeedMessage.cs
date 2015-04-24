

// Generated on 04/24/2015 03:38:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectFeedMessage : Message
    {
        public const uint Id = 6290;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        public int foodUID;
        public int foodQuantity;
        
        public ObjectFeedMessage()
        {
        }
        
        public ObjectFeedMessage(int objectUID, int foodUID, int foodQuantity)
        {
            this.objectUID = objectUID;
            this.foodUID = foodUID;
            this.foodQuantity = foodQuantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(objectUID);
            writer.WriteVarInt(foodUID);
            writer.WriteVarInt(foodQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadVarInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            foodUID = reader.ReadVarInt();
            if (foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + foodUID + ", it doesn't respect the following condition : foodUID < 0");
            foodQuantity = reader.ReadVarInt();
            if (foodQuantity < 0)
                throw new Exception("Forbidden value on foodQuantity = " + foodQuantity + ", it doesn't respect the following condition : foodQuantity < 0");
        }
        
    }
    
}