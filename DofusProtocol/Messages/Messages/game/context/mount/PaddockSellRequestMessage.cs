

// Generated on 04/19/2016 10:17:18
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
        public bool forSale;
        
        public PaddockSellRequestMessage()
        {
        }
        
        public PaddockSellRequestMessage(int price, bool forSale)
        {
            this.price = price;
            this.forSale = forSale;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(price);
            writer.WriteBoolean(forSale);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            forSale = reader.ReadBoolean();
        }
        
    }
    
}