

// Generated on 12/29/2014 21:13:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeClearPaymentForCraftMessage : Message
    {
        public const uint Id = 6145;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte paymentType;
        
        public ExchangeClearPaymentForCraftMessage()
        {
        }
        
        public ExchangeClearPaymentForCraftMessage(sbyte paymentType)
        {
            this.paymentType = paymentType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(paymentType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paymentType = reader.ReadSByte();
            if (paymentType < 0)
                throw new Exception("Forbidden value on paymentType = " + paymentType + ", it doesn't respect the following condition : paymentType < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}