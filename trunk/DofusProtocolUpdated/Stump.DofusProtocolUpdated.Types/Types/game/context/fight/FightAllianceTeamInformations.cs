

// Generated on 03/05/2014 20:34:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightAllianceTeamInformations : FightTeamInformations
    {
        public const short Id = 439;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte relation;
        
        public FightAllianceTeamInformations()
        {
        }
        
        public FightAllianceTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, IEnumerable<Types.FightTeamMemberInformations> teamMembers, sbyte relation)
         : base(teamId, leaderId, teamSide, teamTypeId, teamMembers)
        {
            this.relation = relation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(relation);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            relation = reader.ReadSByte();
            if (relation < 0)
                throw new Exception("Forbidden value on relation = " + relation + ", it doesn't respect the following condition : relation < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}