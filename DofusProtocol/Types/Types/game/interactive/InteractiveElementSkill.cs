

// Generated on 10/26/2014 23:30:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class InteractiveElementSkill
    {
        public const short Id = 219;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int skillId;
        public int skillInstanceUid;
        
        public InteractiveElementSkill()
        {
        }
        
        public InteractiveElementSkill(int skillId, int skillInstanceUid)
        {
            this.skillId = skillId;
            this.skillInstanceUid = skillInstanceUid;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(skillId);
            writer.WriteInt(skillInstanceUid);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            skillId = reader.ReadInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            skillInstanceUid = reader.ReadInt();
            if (skillInstanceUid < 0)
                throw new Exception("Forbidden value on skillInstanceUid = " + skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int);
        }
        
    }
    
}