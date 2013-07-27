

// Generated on 07/26/2013 22:51:00
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ValidateSpellForgetMessage : Message
    {
        public const uint Id = 1700;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short spellId;
        
        public ValidateSpellForgetMessage()
        {
        }
        
        public ValidateSpellForgetMessage(short spellId)
        {
            this.spellId = spellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}