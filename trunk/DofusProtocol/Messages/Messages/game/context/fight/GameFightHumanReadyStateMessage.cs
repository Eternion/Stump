
// Generated on 03/25/2013 19:24:07
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightHumanReadyStateMessage : Message
    {
        public const uint Id = 740;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int characterId;
        public bool isReady;
        
        public GameFightHumanReadyStateMessage()
        {
        }
        
        public GameFightHumanReadyStateMessage(int characterId, bool isReady)
        {
            this.characterId = characterId;
            this.isReady = isReady;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(characterId);
            writer.WriteBoolean(isReady);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            isReady = reader.ReadBoolean();
        }
        
    }
    
}