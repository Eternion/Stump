

// Generated on 08/04/2015 00:35:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterNamedLightInformations : GameFightFighterLightInformations
    {
        public const short Id = 456;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        
        public GameFightFighterNamedLightInformations()
        {
        }
        
        public GameFightFighterNamedLightInformations(bool sex, bool alive, int id, sbyte wave, short level, sbyte breed, string name)
         : base(sex, alive, id, wave, level, breed)
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