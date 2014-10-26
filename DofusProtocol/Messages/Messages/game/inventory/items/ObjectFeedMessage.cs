

// Generated on 10/26/2014 23:29:40
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
        public short foodQuantity;
        
        public ObjectFeedMessage()
        {
        }
        
        public ObjectFeedMessage(int objectUID, int foodUID, short foodQuantity)
        {
            this.objectUID = objectUID;
            this.foodUID = foodUID;
            this.foodQuantity = foodQuantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectUID);
            writer.WriteInt(foodUID);
            writer.WriteShort(foodQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            foodUID = reader.ReadInt();
            if (foodUID < 0)
                throw new Exception("Forbidden value on foodUID = " + foodUID + ", it doesn't respect the following condition : foodUID < 0");
            foodQuantity = reader.ReadShort();
            if (foodQuantity < 0)
                throw new Exception("Forbidden value on foodQuantity = " + foodQuantity + ", it doesn't respect the following condition : foodQuantity < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short);
        }
        
    }
    
}