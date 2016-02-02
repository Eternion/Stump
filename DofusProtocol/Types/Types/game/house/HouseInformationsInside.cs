

// Generated on 02/02/2016 14:14:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformationsInside
    {
        public const short Id = 218;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int houseId;
        public short modelId;
        public int ownerId;
        public string ownerName;
        public short worldX;
        public short worldY;
        public int price;
        public bool isLocked;
        
        public HouseInformationsInside()
        {
        }
        
        public HouseInformationsInside(int houseId, short modelId, int ownerId, string ownerName, short worldX, short worldY, int price, bool isLocked)
        {
            this.houseId = houseId;
            this.modelId = modelId;
            this.ownerId = ownerId;
            this.ownerName = ownerName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.price = price;
            this.isLocked = isLocked;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
            writer.WriteVarShort(modelId);
            writer.WriteInt(ownerId);
            writer.WriteUTF(ownerName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(price);
            writer.WriteBoolean(isLocked);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            modelId = reader.ReadVarShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            ownerId = reader.ReadInt();
            ownerName = reader.ReadUTF();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
            isLocked = reader.ReadBoolean();
        }
        
        
    }
    
}