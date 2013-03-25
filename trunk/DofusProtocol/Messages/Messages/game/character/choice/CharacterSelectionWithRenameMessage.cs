
// Generated on 03/25/2013 19:24:04
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectionWithRenameMessage : CharacterSelectionMessage
    {
        public const uint Id = 6121;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public CharacterSelectionWithRenameMessage()
        {
        }
        
        public CharacterSelectionWithRenameMessage(int id, string name)
         : base(id)
        {
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
        }
        
    }
    
}