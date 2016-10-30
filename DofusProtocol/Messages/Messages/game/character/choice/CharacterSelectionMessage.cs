

// Generated on 10/30/2016 16:20:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectionMessage : Message
    {
        public const uint Id = 152;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long id;
        
        public CharacterSelectionMessage()
        {
        }
        
        public CharacterSelectionMessage(long id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarLong();
            if (id < 0 || id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 9007199254740990");
        }
        
    }
    
}