

// Generated on 10/30/2016 16:20:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamInformations : AbstractFightTeamInformations
    {
        public const short Id = 33;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.FightTeamMemberInformations> teamMembers;
        
        public FightTeamInformations()
        {
        }
        
        public FightTeamInformations(sbyte teamId, double leaderId, sbyte teamSide, sbyte teamTypeId, sbyte nbWaves, IEnumerable<Types.FightTeamMemberInformations> teamMembers)
         : base(teamId, leaderId, teamSide, teamTypeId, nbWaves)
        {
            this.teamMembers = teamMembers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var teamMembers_before = writer.Position;
            var teamMembers_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in teamMembers)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 teamMembers_count++;
            }
            var teamMembers_after = writer.Position;
            writer.Seek((int)teamMembers_before);
            writer.WriteUShort((ushort)teamMembers_count);
            writer.Seek((int)teamMembers_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var teamMembers_ = new Types.FightTeamMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 teamMembers_[i] = Types.ProtocolTypeManager.GetInstance<Types.FightTeamMemberInformations>(reader.ReadShort());
                 teamMembers_[i].Deserialize(reader);
            }
            teamMembers = teamMembers_;
        }
        
        
    }
    
}