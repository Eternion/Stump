

// Generated on 10/30/2016 16:20:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftPaymentModificationRequestMessage : Message
    {
        public const uint Id = 6579;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ExchangeCraftPaymentModificationRequestMessage()
        {
        }
        
        public ExchangeCraftPaymentModificationRequestMessage(int quantity)
        {
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}