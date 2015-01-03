

// Generated on 12/29/2014 21:13:50
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
            writer.WriteShort(genericId);
            writer.WriteInt(baseQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            baseQuantity = reader.ReadInt();
            if (baseQuantity < 0)
                throw new Exception("Forbidden value on baseQuantity = " + baseQuantity + ", it doesn't respect the following condition : baseQuantity < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int);
        }
        
    }
    
}