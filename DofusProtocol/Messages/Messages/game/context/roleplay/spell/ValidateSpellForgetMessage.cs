

// Generated on 12/20/2015 16:36:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            writer.WriteVarShort(spellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellId = reader.ReadVarShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
        }
        
    }
    
}