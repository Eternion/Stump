

// Generated on 11/16/2015 14:26:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockBuyResultMessage : Message
    {
        public const uint Id = 6516;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int paddockId;
        public bool bought;
        public int realPrice;
        
        public PaddockBuyResultMessage()
        {
        }
        
        public PaddockBuyResultMessage(int paddockId, bool bought, int realPrice)
        {
            this.paddockId = paddockId;
            this.bought = bought;
            this.realPrice = realPrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(paddockId);
            writer.WriteBoolean(bought);
            writer.WriteVarInt(realPrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            paddockId = reader.ReadInt();
            bought = reader.ReadBoolean();
            realPrice = reader.ReadVarInt();
            if (realPrice < 0)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0");
        }
        
    }
    
}