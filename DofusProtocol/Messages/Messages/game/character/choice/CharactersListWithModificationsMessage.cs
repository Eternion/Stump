

// Generated on 01/04/2015 11:54:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public CharactersListWithModificationsMessage(IEnumerable<Types.CharacterBaseInformations> characters, bool hasStartupActions, IEnumerable<Types.CharacterToRecolorInformation> charactersToRecolor, IEnumerable<int> charactersToRename, IEnumerable<int> unusableCharacters, IEnumerable<Types.CharacterToRelookInformation> charactersToRelook)
         : base(characters, hasStartupActions)
        {
            this.charactersToRecolor = charactersToRecolor;
            this.charactersToRename = charactersToRename;
            this.unusableCharacters = unusableCharacters;
            this.charactersToRelook = charactersToRelook;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var charactersToRecolor_before = writer.Position;
            var charactersToRecolor_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in charactersToRecolor)
            {
                 entry.Serialize(writer);
                 charactersToRecolor_count++;
            }
            var charactersToRecolor_after = writer.Position;
            writer.Seek((int)charactersToRecolor_before);
            writer.WriteUShort((ushort)charactersToRecolor_count);
            writer.Seek((int)charactersToRecolor_after);

            var charactersToRename_before = writer.Position;
            var charactersToRename_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in charactersToRename)
            {
                 writer.WriteInt(entry);
                 charactersToRename_count++;
            }
            var charactersToRename_after = writer.Position;
            writer.Seek((int)charactersToRename_before);
            writer.WriteUShort((ushort)charactersToRename_count);
            writer.Seek((int)charactersToRename_after);

            var unusableCharacters_before = writer.Position;
            var unusableCharacters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in unusableCharacters)
            {
                 writer.WriteInt(entry);
                 unusableCharacters_count++;
            }
            var unusableCharacters_after = writer.Position;
            writer.Seek((int)unusableCharacters_before);
            writer.WriteUShort((ushort)unusableCharacters_count);
            writer.Seek((int)unusableCharacters_after);

            var charactersToRelook_before = writer.Position;
            var charactersToRelook_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in charactersToRelook)
            {
                 entry.Serialize(writer);
                 charactersToRelook_count++;
            }
            var charactersToRelook_after = writer.Position;
            writer.Seek((int)charactersToRelook_before);
            writer.WriteUShort((ushort)charactersToRelook_count);
            writer.Seek((int)charactersToRelook_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var charactersToRecolor_ = new Types.CharacterToRecolorInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRecolor_[i] = new Types.CharacterToRecolorInformation();
                 charactersToRecolor_[i].Deserialize(reader);
            }
            charactersToRecolor = charactersToRecolor_;
            limit = reader.ReadUShort();
            var charactersToRename_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRename_[i] = reader.ReadInt();
            }
            charactersToRename = charactersToRename_;
            limit = reader.ReadUShort();
            var unusableCharacters_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 unusableCharacters_[i] = reader.ReadInt();
            }
            unusableCharacters = unusableCharacters_;
            limit = reader.ReadUShort();
            var charactersToRelook_ = new Types.CharacterToRelookInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRelook_[i] = new Types.CharacterToRelookInformation();
                 charactersToRelook_[i].Deserialize(reader);
            }
            charactersToRelook = charactersToRelook_;
        }
        
    }
    
}