

// Generated on 02/02/2016 14:14:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObtainedItemMessage : Message
    {
        public const uint Id = 6519;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short genericId;
        public int baseQuantity;
        
        public ObtainedItemMessage()
        {
        }
        
        public ObtainedItemMessage(short genericId, int baseQuantity)
        {
            this.genericId = genericId;
            this.baseQuantity = baseQuantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(genericId);
            writer.WriteVarInt(baseQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadVarShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            baseQuantity = reader.ReadVarInt();
            if (baseQuantity < 0)
                throw new Exception("Forbidden value on baseQuantity = " + baseQuantity + ", it doesn't respect the following condition : baseQuantity < 0");
        }
        
    }
    
}