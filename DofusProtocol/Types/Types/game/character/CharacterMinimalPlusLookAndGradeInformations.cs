

// Generated on 02/02/2016 14:14:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations
    {
        public const short Id = 193;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int grade;
        
        public CharacterMinimalPlusLookAndGradeInformations()
        {
        }
        
        public CharacterMinimalPlusLookAndGradeInformations(long id, byte level, string name, Types.EntityLook entityLook, int grade)
         : base(id, level, name, entityLook)
        {
            this.grade = grade;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(grade);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            grade = reader.ReadVarInt();
            if (grade < 0)
                throw new Exception("Forbidden value on grade = " + grade + ", it doesn't respect the following condition : grade < 0");
        }
        
        
    }
    
}