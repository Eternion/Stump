

// Generated on 02/02/2016 14:14:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseSellRequestMessage : Message
    {
        public const uint Id = 5697;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int amount;
        
        public HouseSellRequestMessage()
        {
        }
        
        public HouseSellRequestMessage(int amount)
        {
            this.amount = amount;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(amount);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            amount = reader.ReadVarInt();
            if (amount < 0)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
        }
        
    }
    
}