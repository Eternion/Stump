

// Generated on 10/30/2016 16:20:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations
    {
        public const short Id = 154;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string name;
        
        public GameRolePlayNamedActorInformations()
        {
        }
        
        public GameRolePlayNamedActorInformations(double contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name)
         : base(contextualId, look, disposition)
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