

// Generated on 11/16/2015 14:26:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftPaymentModifiedMessage : Message
    {
        public const uint Id = 6578;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int goldSum;
        
        public ExchangeCraftPaymentModifiedMessage()
        {
        }
        
        public ExchangeCraftPaymentModifiedMessage(int goldSum)
        {
            this.goldSum = goldSum;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(goldSum);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            goldSum = reader.ReadVarInt();
            if (goldSum < 0)
                throw new Exception("Forbidden value on goldSum = " + goldSum + ", it doesn't respect the following condition : goldSum < 0");
        }
        
    }
    
}