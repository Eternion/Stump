

// Generated on 08/11/2013 11:28:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PurchasableDialogMessage : Message
    {
        public const uint Id = 5739;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool buyOrSell;
        public int purchasableId;
        public int price;
        
        public PurchasableDialogMessage()
        {
        }
        
        public PurchasableDialogMessage(bool buyOrSell, int purchasableId, int price)
        {
            this.buyOrSell = buyOrSell;
            this.purchasableId = purchasableId;
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(buyOrSell);
            writer.WriteInt(purchasableId);
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            buyOrSell = reader.ReadBoolean();
            purchasableId = reader.ReadInt();
            if (purchasableId < 0)
                throw new Exception("Forbidden value on purchasableId = " + purchasableId + ", it doesn't respect the following condition : purchasableId < 0");
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