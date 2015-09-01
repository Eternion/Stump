

// Generated on 09/01/2015 10:48:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectFoundWhileRecoltingMessage : Message
    {
        public const uint Id = 6017;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short genericId;
        public int quantity;
        public int resourceGenericId;
        
        public ObjectFoundWhileRecoltingMessage()
        {
        }
        
        public ObjectFoundWhileRecoltingMessage(short genericId, int quantity, int resourceGenericId)
        {
            this.genericId = genericId;
            this.quantity = quantity;
            this.resourceGenericId = resourceGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(genericId);
            writer.WriteVarInt(quantity);
            writer.WriteVarInt(resourceGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadVarShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            resourceGenericId = reader.ReadVarInt();
            if (resourceGenericId < 0)
                throw new Exception("Forbidden value on resourceGenericId = " + resourceGenericId + ", it doesn't respect the following condition : resourceGenericId < 0");
        }
        
    }
    
}