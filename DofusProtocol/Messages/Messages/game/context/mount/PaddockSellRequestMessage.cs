

// Generated on 10/26/2014 23:29:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockSellRequestMessage : Message
    {
        public const uint Id = 5953;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int price;
        
        public PaddockSellRequestMessage()
        {
        }
        
        public PaddockSellRequestMessage(int price)
        {
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}