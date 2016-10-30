

// Generated on 10/30/2016 16:20:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterBasicMinimalInformations : AbstractCharacterInformation
    {
        public const short Id = 503;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        
        public CharacterBasicMinimalInformations()
        {
        }
        
        public CharacterBasicMinimalInformations(long id, string name)
         : base(id)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }
        
        
    }
    
}