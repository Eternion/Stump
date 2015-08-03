

// Generated on 08/04/2015 00:35:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class InteractiveElementNamedSkill : InteractiveElementSkill
    {
        public const short Id = 220;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int nameId;
        
        public InteractiveElementNamedSkill()
        {
        }
        
        public InteractiveElementNamedSkill(int skillId, int skillInstanceUid, int nameId)
         : base(skillId, skillInstanceUid)
        {
            this.nameId = nameId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(nameId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            nameId = reader.ReadVarInt();
            if (nameId < 0)
                throw new Exception("Forbidden value on nameId = " + nameId + ", it doesn't respect the following condition : nameId < 0");
        }
        
        
    }
    
}