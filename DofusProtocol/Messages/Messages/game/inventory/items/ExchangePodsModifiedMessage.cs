

// Generated on 10/30/2016 16:20:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangePodsModifiedMessage : ExchangeObjectMessage
    {
        public const uint Id = 6670;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int currentWeight;
        public int maxWeight;
        
        public ExchangePodsModifiedMessage()
        {
        }
        
        public ExchangePodsModifiedMessage(bool remote, int currentWeight, int maxWeight)
         : base(remote)
        {
            this.currentWeight = currentWeight;
            this.maxWeight = maxWeight;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(currentWeight);
            writer.WriteVarInt(maxWeight);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            currentWeight = reader.ReadVarInt();
            if (currentWeight < 0)
                throw new Exception("Forbidden value on currentWeight = " + currentWeight + ", it doesn't respect the following condition : currentWeight < 0");
            maxWeight = reader.ReadVarInt();
            if (maxWeight < 0)
                throw new Exception("Forbidden value on maxWeight = " + maxWeight + ", it doesn't respect the following condition : maxWeight < 0");
        }
        
    }
    
}