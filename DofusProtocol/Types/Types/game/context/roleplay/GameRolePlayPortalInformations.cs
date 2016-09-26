

// Generated on 09/26/2016 01:50:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameRolePlayPortalInformations : GameRolePlayActorInformations
    {
        public const short Id = 467;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.PortalInformation portal;
        
        public GameRolePlayPortalInformations()
        {
        }
        
        public GameRolePlayPortalInformations(double contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, Types.PortalInformation portal)
         : base(contextualId, look, disposition)
        {
            this.portal = portal;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(portal.TypeId);
            portal.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            portal = Types.ProtocolTypeManager.GetInstance<Types.PortalInformation>(reader.ReadShort());
            portal.Deserialize(reader);
        }
        
        
    }
    
}