

// Generated on 10/26/2014 23:29:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeShopStockMovementRemovedMessage : Message
    {
        public const uint Id = 5907;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectId;
        
        public ExchangeShopStockMovementRemovedMessage()
        {
        }
        
        public ExchangeShopStockMovementRemovedMessage(int objectId)
        {
            this.objectId = objectId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(objectId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectId = reader.ReadInt();
            if (objectId < 0)
                throw new Exception("Forbidden value on objectId = " + objectId + ", it doesn't respect the following condition : objectId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}