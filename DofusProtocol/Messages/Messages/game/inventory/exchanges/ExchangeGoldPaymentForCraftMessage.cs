

// Generated on 04/24/2015 03:38:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeGoldPaymentForCraftMessage : Message
    {
        public const uint Id = 5833;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public int goldSum;
        
        public ExchangeGoldPaymentForCraftMessage()
        {
        }
        
        public ExchangeGoldPaymentForCraftMessage(bool onlySuccess, int goldSum)
        {
            this.onlySuccess = onlySuccess;
            this.goldSum = goldSum;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            writer.WriteVarInt(goldSum);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            goldSum = reader.ReadVarInt();
            if (goldSum < 0)
                throw new Exception("Forbidden value on goldSum = " + goldSum + ", it doesn't respect the following condition : goldSum < 0");
        }
        
    }
    
}