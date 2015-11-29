

// Generated on 11/16/2015 14:20:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterMinimalAllianceInformations : CharacterMinimalGuildInformations
    {
        public const short Id = 444;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.BasicAllianceInformations alliance;
        
        public CharacterMinimalAllianceInformations()
        {
        }
        
        public CharacterMinimalAllianceInformations(int id, byte level, string name, Types.EntityLook entityLook, Types.BasicGuildInformations guild, Types.BasicAllianceInformations alliance)
         : base(id, level, name, entityLook, guild)
        {
            this.alliance = alliance;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            alliance.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            alliance = new Types.BasicAllianceInformations();
            alliance.Deserialize(reader);
        }
        
        
    }
    
}