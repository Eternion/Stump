

// Generated on 02/18/2015 10:46:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayShowActorMessage : Message
    {
        public const uint Id = 5632;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameRolePlayActorInformations informations;
        
        public GameRolePlayShowActorMessage()
        {
        }
        
        public GameRolePlayShowActorMessage(Types.GameRolePlayActorInformations informations)
        {
            this.informations = informations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            informations = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}