

// Generated on 04/24/2015 03:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseToSellFilterMessage : Message
    {
        public const uint Id = 6137;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int areaId;
        public sbyte atLeastNbRoom;
        public sbyte atLeastNbChest;
        public short skillRequested;
        public int maxPrice;
        
        public HouseToSellFilterMessage()
        {
        }
        
        public HouseToSellFilterMessage(int areaId, sbyte atLeastNbRoom, sbyte atLeastNbChest, short skillRequested, int maxPrice)
        {
            this.areaId = areaId;
            this.atLeastNbRoom = atLeastNbRoom;
            this.atLeastNbChest = atLeastNbChest;
            this.skillRequested = skillRequested;
            this.maxPrice = maxPrice;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(areaId);
            writer.WriteSByte(atLeastNbRoom);
            writer.WriteSByte(atLeastNbChest);
            writer.WriteVarShort(skillRequested);
            writer.WriteVarInt(maxPrice);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            areaId = reader.ReadInt();
            atLeastNbRoom = reader.ReadSByte();
            if (atLeastNbRoom < 0)
                throw new Exception("Forbidden value on atLeastNbRoom = " + atLeastNbRoom + ", it doesn't respect the following condition : atLeastNbRoom < 0");
            atLeastNbChest = reader.ReadSByte();
            if (atLeastNbChest < 0)
                throw new Exception("Forbidden value on atLeastNbChest = " + atLeastNbChest + ", it doesn't respect the following condition : atLeastNbChest < 0");
            skillRequested = reader.ReadVarShort();
            if (skillRequested < 0)
                throw new Exception("Forbidden value on skillRequested = " + skillRequested + ", it doesn't respect the following condition : skillRequested < 0");
            maxPrice = reader.ReadVarInt();
            if (maxPrice < 0)
                throw new Exception("Forbidden value on maxPrice = " + maxPrice + ", it doesn't respect the following condition : maxPrice < 0");
        }
        
    }
    
}