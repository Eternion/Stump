

// Generated on 03/05/2014 20:34:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyCompanionMemberInformations : PartyCompanionBaseInformations
    {
        public const short Id = 452;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short initiative;
        public int lifePoints;
        public int maxLifePoints;
        public short prospecting;
        public byte regenRate;
        
        public PartyCompanionMemberInformations()
        {
        }
        
        public PartyCompanionMemberInformations(sbyte indexId, short companionGenericId, Types.EntityLook entityLook, short initiative, int lifePoints, int maxLifePoints, short prospecting, byte regenRate)
         : base(indexId, companionGenericId, entityLook)
        {
            this.initiative = initiative;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(initiative);
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteShort(prospecting);
            writer.WriteByte(regenRate);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            initiative = reader.ReadShort();
            if (initiative < 0)
                throw new Exception("Forbidden value on initiative = " + initiative + ", it doesn't respect the following condition : initiative < 0");
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
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(int) + sizeof(int) + sizeof(short) + sizeof(byte);
        }
        
    }
    
}