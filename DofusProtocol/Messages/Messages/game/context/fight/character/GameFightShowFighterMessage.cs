

// Generated on 02/02/2016 14:14:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightShowFighterMessage : Message
    {
        public const uint Id = 5864;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameFightFighterInformations informations;
        
        public GameFightShowFighterMessage()
        {
        }
        
        public GameFightShowFighterMessage(Types.GameFightFighterInformations informations)
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
            informations = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
            informations.Deserialize(reader);
        }
        
    }
    
}