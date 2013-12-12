

// Generated on 12/12/2013 16:57:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AbstractFightTeamInformations
    {
        public const short Id = 116;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte teamId;
        public int leaderId;
        public sbyte teamSide;
        public sbyte teamTypeId;
        
        public AbstractFightTeamInformations()
        {
        }
        
        public AbstractFightTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId)
        {
            this.teamId = teamId;
            this.leaderId = leaderId;
            this.teamSide = teamSide;
            this.teamTypeId = teamTypeId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(teamId);
            writer.WriteInt(leaderId);
            writer.WriteSByte(teamSide);
            writer.WriteSByte(teamTypeId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            teamId = reader.ReadSByte();
            if (teamId < 0)
                throw new Exception("Forbidden value on teamId = " + teamId + ", it doesn't respect the following condition : teamId < 0");
            leaderId = reader.ReadInt();
            teamSide = reader.ReadSByte();
            teamTypeId = reader.ReadSByte();
            if (teamTypeId < 0)
                throw new Exception("Forbidden value on teamTypeId = " + teamTypeId + ", it doesn't respect the following condition : teamTypeId < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(int) + sizeof(sbyte) + sizeof(sbyte);
        }
        
    }
    
}