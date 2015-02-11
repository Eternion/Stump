

// Generated on 02/11/2015 10:20:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeItemGoldAddAsPaymentMessage : Message
    {
        public const uint Id = 5770;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte paymentType;
        public int quantity;
        
        public ExchangeItemGoldAddAsPaymentMessage()
        {
        }
        
        public ExchangeItemGoldAddAsPaymentMessage(sbyte paymentType, int quantity)
        {
            this.paymentType = paymentType;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(paymentType);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paymentType = reader.ReadSByte();
            if (paymentType < 0)
                throw new Exception("Forbidden value on paymentType = " + paymentType + ", it doesn't respect the following condition : paymentType < 0");
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}