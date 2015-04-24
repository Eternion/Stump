

// Generated on 04/24/2015 03:38:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyInvitationMemberInformations : CharacterBaseInformations
    {
        public const short Id = 376;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public IEnumerable<Types.PartyCompanionBaseInformations> companions;
        
        public PartyInvitationMemberInformations()
        {
        }
        
        public PartyInvitationMemberInformations(int id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex, short worldX, short worldY, int mapId, short subAreaId, IEnumerable<Types.PartyCompanionBaseInformations> companions)
         : base(id, level, name, entityLook, breed, sex)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.companions = companions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarShort(subAreaId);
            var companions_before = writer.Position;
            var companions_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in companions)
            {
                 entry.Serialize(writer);
                 companions_count++;
            }
            var companions_after = writer.Position;
            writer.Seek((int)companions_before);
            writer.WriteUShort((ushort)companions_count);
            writer.Seek((int)companions_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
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
            var companions_ = new Types.PartyCompanionBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 companions_[i] = new Types.PartyCompanionBaseInformations();
                 companions_[i].Deserialize(reader);
            }
            companions = companions_;
        }
        
        
    }
    
}