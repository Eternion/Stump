

// Generated on 04/24/2015 03:37:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharactersListWithRemodelingMessage : CharactersListMessage
    {
        public const uint Id = 6550;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.CharacterToRemodelInformations> charactersToRemodel;
        
        public CharactersListWithRemodelingMessage()
        {
        }
        
        public CharactersListWithRemodelingMessage(IEnumerable<Types.CharacterBaseInformations> characters, bool hasStartupActions, IEnumerable<Types.CharacterToRemodelInformations> charactersToRemodel)
         : base(characters, hasStartupActions)
        {
            this.charactersToRemodel = charactersToRemodel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var charactersToRemodel_before = writer.Position;
            var charactersToRemodel_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in charactersToRemodel)
            {
                 entry.Serialize(writer);
                 charactersToRemodel_count++;
            }
            var charactersToRemodel_after = writer.Position;
            writer.Seek((int)charactersToRemodel_before);
            writer.WriteUShort((ushort)charactersToRemodel_count);
            writer.Seek((int)charactersToRemodel_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var charactersToRemodel_ = new Types.CharacterToRemodelInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersToRemodel_[i] = new Types.CharacterToRemodelInformations();
                 charactersToRemodel_[i].Deserialize(reader);
            }
            charactersToRemodel = charactersToRemodel_;
        }
        
    }
    
}