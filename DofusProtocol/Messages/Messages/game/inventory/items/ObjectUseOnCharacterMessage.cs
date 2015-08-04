

// Generated on 08/04/2015 13:25:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectUseOnCharacterMessage : ObjectUseMessage
    {
        public const uint Id = 3003;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int characterId;
        
        public ObjectUseOnCharacterMessage()
        {
        }
        
        public ObjectUseOnCharacterMessage(int objectUID, int characterId)
         : base(objectUID)
        {
            this.characterId = characterId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(characterId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            characterId = reader.ReadVarInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
        }
        
    }
    
}