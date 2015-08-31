

// Generated on 08/04/2015 00:35:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PaddockBuyableInformations : PaddockInformations
    {
        public const short Id = 130;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int price;
        public bool locked;
        
        public PaddockBuyableInformations()
        {
        }
        
        public PaddockBuyableInformations(short maxOutdoorMount, short maxItems, int price, bool locked)
         : base(maxOutdoorMount, maxItems)
        {
            this.price = price;
            this.locked = locked;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(price);
            writer.WriteBoolean(locked);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadVarInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            locked = reader.ReadBoolean();
        }
        
        
    }
    
}