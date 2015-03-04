

// Generated on 02/19/2015 12:09:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryWeightMessage : Message
    {
        public const uint Id = 3009;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int weight;
        public int weightMax;
        
        public InventoryWeightMessage()
        {
        }
        
        public InventoryWeightMessage(int weight, int weightMax)
        {
            this.weight = weight;
            this.weightMax = weightMax;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(weight);
            writer.WriteVarInt(weightMax);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            weight = reader.ReadVarInt();
            if (weight < 0)
                throw new Exception("Forbidden value on weight = " + weight + ", it doesn't respect the following condition : weight < 0");
            weightMax = reader.ReadVarInt();
            if (weightMax < 0)
                throw new Exception("Forbidden value on weightMax = " + weightMax + ", it doesn't respect the following condition : weightMax < 0");
        }
        
    }
    
}