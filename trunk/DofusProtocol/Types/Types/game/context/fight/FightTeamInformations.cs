

// Generated on 07/26/2013 22:51:10
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public FightTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, IEnumerable<Types.FightTeamMemberInformations> teamMembers)
         : base(teamId, leaderId, teamSide, teamTypeId)
        {
            this.teamMembers = teamMembers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)teamMembers.Count());
            foreach (var entry in teamMembers)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            teamMembers = new Types.FightTeamMemberInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (teamMembers as Types.FightTeamMemberInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.FightTeamMemberInformations>(reader.ReadShort());
                 (teamMembers as Types.FightTeamMemberInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + teamMembers.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}