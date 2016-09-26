

// Generated on 09/26/2016 01:50:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PartyMemberArenaInformations : PartyMemberInformations
    {
        public const short Id = 391;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short rank;
        
        public PartyMemberArenaInformations()
        {
        }
        
        public PartyMemberArenaInformations(long id, string name, byte level, Types.EntityLook entityLook, sbyte breed, bool sex, int lifePoints, int maxLifePoints, short prospecting, byte regenRate, short initiative, sbyte alignmentSide, short worldX, short worldY, int mapId, short subAreaId, Types.PlayerStatus status, IEnumerable<Types.PartyCompanionMemberInformations> companions, short rank)
         : base(id, name, level, entityLook, breed, sex, lifePoints, maxLifePoints, prospecting, regenRate, initiative, alignmentSide, worldX, worldY, mapId, subAreaId, status, companions)
        {
            this.rank = rank;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(rank);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            rank = reader.ReadVarShort();
            if (rank < 0 || rank > 20000)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0 || rank > 20000");
        }
        
        
    }
    
}