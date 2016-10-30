

// Generated on 10/30/2016 16:20:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemGenericQuantityPrice : ObjectItemGenericQuantity
    {
        public const short Id = 494;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int price;
        
        public ObjectItemGenericQuantityPrice()
        {
        }
        
        public ObjectItemGenericQuantityPrice(short objectGID, int quantity, int price)
         : base(objectGID, quantity)
        {
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
        
    }
    
}