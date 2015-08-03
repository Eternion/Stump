

// Generated on 08/04/2015 00:35:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class SkillActionDescription
    {
        public const short Id = 102;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short skillId;
        
        public SkillActionDescription()
        {
        }
        
        public SkillActionDescription(short skillId)
        {
            this.skillId = skillId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(skillId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            skillId = reader.ReadVarShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
        
    }
    
}