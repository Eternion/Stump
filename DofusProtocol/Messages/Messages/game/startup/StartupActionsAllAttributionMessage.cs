

// Generated on 04/19/2016 10:17:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionsAllAttributionMessage : Message
    {
        public const uint Id = 6537;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long characterId;
        
        public StartupActionsAllAttributionMessage()
        {
        }
        
        public StartupActionsAllAttributionMessage(long characterId)
        {
            this.characterId = characterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(characterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            characterId = reader.ReadVarLong();
            if (characterId < 0 || characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
        }
        
    }
    
}