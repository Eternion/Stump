

// Generated on 03/02/2014 20:42:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharactersListMessage : Message
    {
        public const uint Id = 151;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool hasStartupActions;
        public IEnumerable<Types.CharacterBaseInformations> characters;
        
        public CharactersListMessage()
        {
        }
        
        public CharactersListMessage(bool hasStartupActions, IEnumerable<Types.CharacterBaseInformations> characters)
        {
            this.hasStartupActions = hasStartupActions;
            this.characters = characters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(hasStartupActions);
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
            hasStartupActions = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var characters_ = new Types.CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 characters_[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterBaseInformations>(reader.ReadShort());
                 characters_[i].Deserialize(reader);
            }
            characters = characters_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(short) + characters.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}