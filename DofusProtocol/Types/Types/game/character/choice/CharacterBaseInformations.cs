

// Generated on 10/30/2016 16:20:53
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
        
        public CharacterBaseInformations(long id, string name, byte level, Types.EntityLook entityLook, sbyte breed, bool sex)
         : base(id, name, level, entityLook)
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