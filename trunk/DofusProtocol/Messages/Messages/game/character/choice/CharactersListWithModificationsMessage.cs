

// Generated on 07/26/2013 22:50:52
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharactersListWithModificationsMessage : CharactersListMessage
    {
        public const uint Id = 6120;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.CharacterToRecolorInformation> charactersToRecolor;
        public IEnumerable<int> charactersToRename;
        public IEnumerable<int> unusableCharacters;
        public IEnumerable<Types.CharacterToRelookInformation> charactersToRelook;
        
        public CharactersListWithModificationsMessage()
        {
        }
        
        public CharactersListWithModificationsMessage(bool hasStartupActions, IEnumerable<Types.CharacterBaseInformations> characters, IEnumerable<Types.CharacterToRecolorInformation> charactersToRecolor, IEnumerable<int> charactersToRename, IEnumerable<int> unusableCharacters, IEnumerable<Types.CharacterToRelookInformation> charactersToRelook)
         : base(hasStartupActions, characters)
        {
            this.charactersToRecolor = charactersToRecolor;
            this.charactersToRename = charactersToRename;
            this.unusableCharacters = unusableCharacters;
            this.charactersToRelook = charactersToRelook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)charactersToRecolor.Count());
            foreach (var entry in charactersToRecolor)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)charactersToRename.Count());
            foreach (var entry in charactersToRename)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)unusableCharacters.Count());
            foreach (var entry in unusableCharacters)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)charactersToRelook.Count());
            foreach (var entry in charactersToRelook)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            charactersToRecolor = new Types.CharacterToRecolorInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (charactersToRecolor as Types.CharacterToRecolorInformation[])[i] = new Types.CharacterToRecolorInformation();
                 (charactersToRecolor as Types.CharacterToRecolorInformation[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            charactersToRename = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (charactersToRename as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            unusableCharacters = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (unusableCharacters as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            charactersToRelook = new Types.CharacterToRelookInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 (charactersToRelook as Types.CharacterToRelookInformation[])[i] = new Types.CharacterToRelookInformation();
                 (charactersToRelook as Types.CharacterToRelookInformation[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + charactersToRecolor.Sum(x => x.GetSerializationSize()) + sizeof(short) + charactersToRename.Sum(x => sizeof(int)) + sizeof(short) + unusableCharacters.Sum(x => sizeof(int)) + sizeof(short) + charactersToRelook.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}