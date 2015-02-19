

// Generated on 02/18/2015 11:05:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformationsForGuild
    {
        public const short Id = 170;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int houseId;
        public int modelId;
        public string ownerName;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public IEnumerable<int> skillListIds;
        public int guildshareParams;
        
        public HouseInformationsForGuild()
        {
        }
        
        public HouseInformationsForGuild(int houseId, int modelId, string ownerName, short worldX, short worldY, int mapId, short subAreaId, IEnumerable<int> skillListIds, int guildshareParams)
        {
            this.houseId = houseId;
            this.modelId = modelId;
            this.ownerName = ownerName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.skillListIds = skillListIds;
            this.guildshareParams = guildshareParams;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
            writer.WriteVarInt(modelId);
            writer.WriteUTF(ownerName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarShort(subAreaId);
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

            writer.WriteVarInt(guildshareParams);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            modelId = reader.ReadVarInt();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
            ownerName = reader.ReadUTF();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            var limit = reader.ReadUShort();
            var skillListIds_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 skillListIds_[i] = reader.ReadInt();
            }
            skillListIds = skillListIds_;
            guildshareParams = reader.ReadVarInt();
            if (guildshareParams < 0)
                throw new Exception("Forbidden value on guildshareParams = " + guildshareParams + ", it doesn't respect the following condition : guildshareParams < 0");
        }
        
        
    }
    
}