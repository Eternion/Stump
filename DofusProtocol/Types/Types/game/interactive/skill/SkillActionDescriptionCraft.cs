

// Generated on 09/01/2015 10:48:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class SkillActionDescriptionCraft : SkillActionDescription
    {
        public const short Id = 100;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte probability;
        
        public SkillActionDescriptionCraft()
        {
        }
        
        public SkillActionDescriptionCraft(short skillId, sbyte probability)
         : base(skillId)
        {
            this.probability = probability;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(probability);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            probability = reader.ReadSByte();
            if (probability < 0)
                throw new Exception("Forbidden value on probability = " + probability + ", it doesn't respect the following condition : probability < 0");
        }
        
        
    }
    
}