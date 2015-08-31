

// Generated on 08/04/2015 13:25:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectUseInWorkshopMessage : Message
    {
        public const uint Id = 6004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        public int quantity;
        
        public ExchangeObjectUseInWorkshopMessage()
        {
        }
        
        public ExchangeObjectUseInWorkshopMessage(int objectUID, int quantity)
        {
            this.objectUID = objectUID;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(objectUID);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadVarInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadVarInt();
        }
        
    }
    
}