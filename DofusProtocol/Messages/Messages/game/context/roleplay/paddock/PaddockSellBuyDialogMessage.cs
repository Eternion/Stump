

// Generated on 12/29/2014 21:12:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockSellBuyDialogMessage : Message
    {
        public const uint Id = 6018;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool bsell;
        public int ownerId;
        public int price;
        
        public PaddockSellBuyDialogMessage()
        {
        }
        
        public PaddockSellBuyDialogMessage(bool bsell, int ownerId, int price)
        {
            this.bsell = bsell;
            this.ownerId = ownerId;
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(bsell);
            writer.WriteInt(ownerId);
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            bsell = reader.ReadBoolean();
            ownerId = reader.ReadInt();
            if (ownerId < 0)
                throw new Exception("Forbidden value on ownerId = " + ownerId + ", it doesn't respect the following condition : ownerId < 0");
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(int) + sizeof(int);
        }
        
    }
    
}