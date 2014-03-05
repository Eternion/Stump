

// Generated on 03/05/2014 20:34:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformationsForSell
    {
        public const short Id = 221;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int modelId;
        public string ownerName;
        public bool ownerConnected;
        public short worldX;
        public short worldY;
        public short subAreaId;
        public sbyte nbRoom;
        public sbyte nbChest;
        public IEnumerable<int> skillListIds;
        public bool isLocked;
        public int price;
        
        public HouseInformationsForSell()
        {
        }
        
        public HouseInformationsForSell(int modelId, string ownerName, bool ownerConnected, short worldX, short worldY, short subAreaId, sbyte nbRoom, sbyte nbChest, IEnumerable<int> skillListIds, bool isLocked, int price)
        {
            this.modelId = modelId;
            this.ownerName = ownerName;
            this.ownerConnected = ownerConnected;
            this.worldX = worldX;
            this.worldY = worldY;
            this.subAreaId = subAreaId;
            this.nbRoom = nbRoom;
            this.nbChest = nbChest;
            this.skillListIds = skillListIds;
            this.isLocked = isLocked;
            this.price = price;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(modelId);
            writer.WriteUTF(ownerName);
            writer.WriteBoolean(ownerConnected);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteShort(subAreaId);
            writer.WriteSByte(nbRoom);
            writer.WriteSByte(nbChest);
            var skillListIds_before = writer.Position;
            var skillListIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in skillListIds)
            {
                 writer.WriteInt(entry);
                 skillListIds_count++;
            }
            var skillListIds_after = writer.Position;
            writer.Seek((int)skillListIds_before);
            writer.WriteUShort((ushort)skillListIds_count);
            writer.Seek((int)skillListIds_after);

            writer.WriteBoolean(isLocked);
            writer.WriteInt(price);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            modelId = reader.ReadInt();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            ownerName = reader.ReadUTF();
            ownerConnected = reader.ReadBoolean();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            nbRoom = reader.ReadSByte();
            nbChest = reader.ReadSByte();
            var limit = reader.ReadUShort();
            var skillListIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 skillListIds_[i] = reader.ReadInt();
            }
            skillListIds = skillListIds_;
            isLocked = reader.ReadBoolean();
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(ownerName) + sizeof(bool) + sizeof(short) + sizeof(short) + sizeof(short) + sizeof(sbyte) + sizeof(sbyte) + sizeof(short) + skillListIds.Sum(x => sizeof(int)) + sizeof(bool) + sizeof(int);
        }
        
    }
    
}