

// Generated on 03/05/2014 20:34:28
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
            writer.WriteInt(houseId);
            writer.WriteInt(realPrice);
            writer.WriteUTF(buyerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            realPrice = reader.ReadInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
            buyerName = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(buyerName);
        }
        
    }
    
}