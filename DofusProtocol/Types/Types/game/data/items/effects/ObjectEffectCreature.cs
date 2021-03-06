

// Generated on 10/30/2016 16:20:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectEffectCreature : ObjectEffect
    {
        public const short Id = 71;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short monsterFamilyId;
        
        public ObjectEffectCreature()
        {
        }
        
        public ObjectEffectCreature(short actionId, short monsterFamilyId)
         : base(actionId)
        {
            this.monsterFamilyId = monsterFamilyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort(monsterFamilyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            monsterFamilyId = reader.ReadVarShort();
            if (monsterFamilyId < 0)
                throw new Exception("Forbidden value on monsterFamilyId = " + monsterFamilyId + ", it doesn't respect the following condition : monsterFamilyId < 0");
        }
        
        
    }
    
}