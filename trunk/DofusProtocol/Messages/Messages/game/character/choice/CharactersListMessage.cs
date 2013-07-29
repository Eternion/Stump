

// Generated on 07/29/2013 23:07:40
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)characters.Count());
            foreach (var entry in characters)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            hasStartupActions = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            characters = new Types.CharacterBaseInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (characters as Types.CharacterBaseInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterBaseInformations>(reader.ReadShort());
                 (characters as Types.CharacterBaseInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(short) + characters.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}