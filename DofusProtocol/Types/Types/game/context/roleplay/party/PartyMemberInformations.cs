

// Generated on 12/29/2014 21:14:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyMemberInformations : CharacterBaseInformations
    {
        public const short Id = 90;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int lifePoints;
        public int maxLifePoints;
        public short prospecting;
        public byte regenRate;
        public short initiative;
        public sbyte alignmentSide;
        public short worldX;
        public short worldY;
        public int mapId;
        public short subAreaId;
        public Types.PlayerStatus status;
        public IEnumerable<Types.PartyCompanionMemberInformations> companions;
        
        public PartyMemberInformations()
        {
        }
        
        public PartyMemberInformations(int id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex, int lifePoints, int maxLifePoints, short prospecting, byte regenRate, short initiative, sbyte alignmentSide, short worldX, short worldY, int mapId, short subAreaId, Types.PlayerStatus status, IEnumerable<Types.PartyCompanionMemberInformations> companions)
         : base(id, level, name, entityLook, breed, sex)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
            this.initiative = initiative;
            this.alignmentSide = alignmentSide;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.status = status;
            this.companions = companions;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteShort(prospecting);
            writer.WriteByte(regenRate);
            writer.WriteShort(initiative);
            writer.WriteSByte(alignmentSide);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteShort(subAreaId);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
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
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            prospecting = reader.ReadShort();
            if (prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + prospecting + ", it doesn't respect the following condition : prospecting < 0");
            regenRate = reader.ReadByte();
            if (regenRate < 0 || regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
            initiative = reader.ReadShort();
            if (initiative < 0)
                throw new Exception("Forbidden value on initiative = " + initiative + ", it doesn't respect the following condition : initiative < 0");
            alignmentSide = reader.ReadSByte();
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
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
            var limit = reader.ReadUShort();
            var companions_ = new Types.PartyCompanionMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 companions_[i] = new Types.PartyCompanionMemberInformations();
                 companions_[i].Deserialize(reader);
            }
            companions = companions_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(int) + sizeof(short) + sizeof(byte) + sizeof(short) + sizeof(sbyte) + sizeof(short) + sizeof(short) + sizeof(int) + sizeof(short) + sizeof(short) + status.GetSerializationSize() + sizeof(short) + companions.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}