

// Generated on 12/29/2014 21:13:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidPriceMessage : Message
    {
        public const uint Id = 5755;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short genericId;
        public int averagePrice;
        
        public ExchangeBidPriceMessage()
        {
        }
        
        public ExchangeBidPriceMessage(short genericId, int averagePrice)
        {
            this.genericId = genericId;
            this.averagePrice = averagePrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(genericId);
            writer.WriteInt(averagePrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genericId = reader.ReadShort();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            averagePrice = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(int);
        }
        
    }
    
}