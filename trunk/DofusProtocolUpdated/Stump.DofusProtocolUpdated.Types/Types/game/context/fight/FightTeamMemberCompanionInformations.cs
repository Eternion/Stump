

// Generated on 12/12/2013 16:57:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightTeamMemberCompanionInformations : FightTeamMemberInformations
    {
        public const short Id = 451;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int companionId;
        public short level;
        
        public FightTeamMemberCompanionInformations()
        {
        }
        
        public FightTeamMemberCompanionInformations(int id, int companionId, short level)
         : base(id)
        {
            this.companionId = companionId;
            this.level = level;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(companionId);
            writer.WriteShort(level);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            companionId = reader.ReadInt();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(short);
        }
        
    }
    
}