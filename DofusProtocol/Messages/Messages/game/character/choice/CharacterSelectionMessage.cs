

// Generated on 08/04/2015 00:36:57
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
        
        public int id;
        
        public CharacterSelectionMessage()
        {
        }
        
        public CharacterSelectionMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            if (id < 1 || id > 2147483647)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 1 || id > 2147483647");
        }
        
    }
    
}