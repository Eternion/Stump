

// Generated on 10/26/2014 23:29:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterReplayWithRenameRequestMessage : CharacterReplayRequestMessage
    {
        public const uint Id = 6122;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        
        public CharacterReplayWithRenameRequestMessage()
        {
        }
        
        public CharacterReplayWithRenameRequestMessage(int characterId, string name)
         : base(characterId)
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
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + Encoding.UTF8.GetByteCount(name);
        }
        
    }
    
}