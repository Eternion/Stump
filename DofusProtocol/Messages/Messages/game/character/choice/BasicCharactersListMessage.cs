

// Generated on 02/02/2016 14:14:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicCharactersListMessage : Message
    {
        public const uint Id = 6475;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.CharacterBaseInformations> characters;
        
        public BasicCharactersListMessage()
        {
        }
        
        public BasicCharactersListMessage(IEnumerable<Types.CharacterBaseInformations> characters)
        {
            this.characters = characters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var characters_before = writer.Position;
            var characters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in characters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 characters_count++;
            }
            var characters_after = writer.Position;
            writer.Seek((int)characters_before);
            writer.WriteUShort((ushort)characters_count);
            writer.Seek((int)characters_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var characters_ = new Types.CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 characters_[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterBaseInformations>(reader.ReadShort());
                 characters_[i].Deserialize(reader);
            }
            characters = characters_;
        }
        
    }
    
}