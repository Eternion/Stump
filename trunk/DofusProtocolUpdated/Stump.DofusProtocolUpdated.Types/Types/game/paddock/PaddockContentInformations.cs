

// Generated on 03/05/2014 20:34:51
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PaddockContentInformations : PaddockInformations
    {
        public const short Id = 183;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int paddockId;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public bool abandonned;
        public IEnumerable<Types.MountInformationsForPaddock> mountsInformations;
        
        public PaddockContentInformations()
        {
        }
        
        public PaddockContentInformations(short maxOutdoorMount, short maxItems, int paddockId, short worldX, short worldY, int mapId, short subAreaId, bool abandonned, IEnumerable<Types.MountInformationsForPaddock> mountsInformations)
         : base(maxOutdoorMount, maxItems)
        {
            this.paddockId = paddockId;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.abandonned = abandonned;
            this.mountsInformations = mountsInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(paddockId);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteShort(subAreaId);
            writer.WriteBoolean(abandonned);
            var mountsInformations_before = writer.Position;
            var mountsInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in mountsInformations)
            {
                 entry.Serialize(writer);
                 mountsInformations_count++;
            }
            var mountsInformations_after = writer.Position;
            writer.Seek((int)mountsInformations_before);
            writer.WriteUShort((ushort)mountsInformations_count);
            writer.Seek((int)mountsInformations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            paddockId = reader.ReadInt();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            abandonned = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var mountsInformations_ = new Types.MountInformationsForPaddock[limit];
            for (int i = 0; i < limit; i++)
            {
                 mountsInformations_[i] = new Types.MountInformationsForPaddock();
                 mountsInformations_[i].Deserialize(reader);
            }
            mountsInformations = mountsInformations_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short) + sizeof(short) + sizeof(int) + sizeof(short) + sizeof(bool) + sizeof(short) + mountsInformations.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}