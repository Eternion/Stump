

// Generated on 12/29/2014 21:13:48
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
            writer.WriteShort(genericId);
            writer.WriteInt(quantity);
            writer.WriteInt(resourceGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            resourceGenericId = reader.ReadInt();
            if (resourceGenericId < 0)
                throw new Exception("Forbidden value on resourceGenericId = " + resourceGenericId + ", it doesn't respect the following condition : resourceGenericId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int) + sizeof(int);
        }
        
    }
    
}