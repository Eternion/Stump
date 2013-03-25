
// Generated on 03/25/2013 19:24:08
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnReadyMessage : Message
    {
        public const uint Id = 716;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool isReady;
        
        public GameFightTurnReadyMessage()
        {
        }
        
        public GameFightTurnReadyMessage(bool isReady)
        {
            this.isReady = isReady;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(isReady);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            isReady = reader.ReadBoolean();
        }
        
    }
    
}