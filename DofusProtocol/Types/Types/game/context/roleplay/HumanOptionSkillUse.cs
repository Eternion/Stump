

// Generated on 10/30/2016 16:20:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOptionSkillUse : HumanOption
    {
        public const short Id = 495;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int elementId;
        public short skillId;
        public double skillEndTime;
        
        public HumanOptionSkillUse()
        {
        }
        
        public HumanOptionSkillUse(int elementId, short skillId, double skillEndTime)
        {
            this.elementId = elementId;
            this.skillId = skillId;
            this.skillEndTime = skillEndTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(elementId);
            writer.WriteVarShort(skillId);
            writer.WriteDouble(skillEndTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            elementId = reader.ReadVarInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            skillId = reader.ReadVarShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            skillEndTime = reader.ReadDouble();
            if (skillEndTime < -9007199254740990 || skillEndTime > 9007199254740990)
                throw new Exception("Forbidden value on skillEndTime = " + skillEndTime + ", it doesn't respect the following condition : skillEndTime < -9007199254740990 || skillEndTime > 9007199254740990");
        }
        
        
    }
    
}