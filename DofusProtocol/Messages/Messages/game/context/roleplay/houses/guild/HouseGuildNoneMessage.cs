

// Generated on 09/01/2015 10:48:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseGuildNoneMessage : Message
    {
        public const uint Id = 5701;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        
        public HouseGuildNoneMessage()
        {
        }
        
        public HouseGuildNoneMessage(int houseId)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
        }
        
    }
    
}