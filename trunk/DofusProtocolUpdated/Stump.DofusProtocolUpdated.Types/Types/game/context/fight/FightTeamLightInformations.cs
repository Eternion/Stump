

// Generated on 12/12/2013 16:57:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamLightInformations : AbstractFightTeamInformations
    {
        public const short Id = 115;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte teamMembersCount;
        public int meanLevel;
        
        public FightTeamLightInformations()
        {
        }
        
        public FightTeamLightInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, sbyte teamMembersCount, int meanLevel)
         : base(teamId, leaderId, teamSide, teamTypeId)
        {
            this.teamMembersCount = teamMembersCount;
            this.meanLevel = meanLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(teamMembersCount);
            writer.WriteInt(meanLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            teamMembersCount = reader.ReadSByte();
            if (teamMembersCount < 0)
                throw new Exception("Forbidden value on teamMembersCount = " + teamMembersCount + ", it doesn't respect the following condition : teamMembersCount < 0");
            meanLevel = reader.ReadInt();
            if (meanLevel < 0)
                throw new Exception("Forbidden value on meanLevel = " + meanLevel + ", it doesn't respect the following condition : meanLevel < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(sbyte) + sizeof(int);
        }
        
    }
    
}