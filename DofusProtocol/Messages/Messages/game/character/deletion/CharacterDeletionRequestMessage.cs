

// Generated on 10/30/2016 16:20:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterDeletionRequestMessage : Message
    {
        public const uint Id = 165;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long characterId;
        public string secretAnswerHash;
        
        public CharacterDeletionRequestMessage()
        {
        }
        
        public CharacterDeletionRequestMessage(long characterId, string secretAnswerHash)
        {
            this.characterId = characterId;
            this.secretAnswerHash = secretAnswerHash;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(characterId);
            writer.WriteUTF(secretAnswerHash);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            characterId = reader.ReadVarLong();
            if (characterId < 0 || characterId > 9007199254740990)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0 || characterId > 9007199254740990");
            secretAnswerHash = reader.ReadUTF();
        }
        
    }
    
}