

// Generated on 10/30/2016 16:20:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterMinimalGuildInformations : CharacterMinimalPlusLookInformations
    {
        public const short Id = 445;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.BasicGuildInformations guild;
        
        public CharacterMinimalGuildInformations()
        {
        }
        
        public CharacterMinimalGuildInformations(long id, string name, byte level, Types.EntityLook entityLook, Types.BasicGuildInformations guild)
         : base(id, name, level, entityLook)
        {
            this.guild = guild;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guild.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guild = new Types.BasicGuildInformations();
            guild.Deserialize(reader);
        }
        
        
    }
    
}