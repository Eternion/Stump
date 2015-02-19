

// Generated on 02/18/2015 11:05:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterBaseInformations : CharacterMinimalPlusLookInformations
    {
        public const short Id = 45;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte breed;
        public bool sex;
        
        public CharacterBaseInformations()
        {
        }
        
        public CharacterBaseInformations(int id, byte level, string name, Types.EntityLook entityLook, sbyte breed, bool sex)
         : base(id, level, name, entityLook)
        {
            this.breed = breed;
            this.sex = sex;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            breed = reader.ReadSByte();
            sex = reader.ReadBoolean();
        }
        
        
    }
    
}