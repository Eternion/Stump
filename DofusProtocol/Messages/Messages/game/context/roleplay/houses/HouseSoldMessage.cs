

// Generated on 02/19/2015 12:09:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseSoldMessage : Message
    {
        public const uint Id = 5737;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        public int realPrice;
        public string buyerName;
        
        public HouseSoldMessage()
        {
        }
        
        public HouseSoldMessage(int houseId, int realPrice, string buyerName)
        {
            this.houseId = houseId;
            this.realPrice = realPrice;
            this.buyerName = buyerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
            writer.WriteVarInt(realPrice);
            writer.WriteUTF(buyerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            realPrice = reader.ReadVarInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
            buyerName = reader.ReadUTF();
        }
        
    }
    
}