

// Generated on 03/02/2014 20:42:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightAttackerAddMessage : Message
    {
        public const uint Id = 5893;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> charactersDescription;
        
        public PrismFightAttackerAddMessage()
        {
        }
        
        public PrismFightAttackerAddMessage(double fightId, IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> charactersDescription)
        {
            this.fightId = fightId;
            this.charactersDescription = charactersDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            var charactersDescription_before = writer.Position;
            var charactersDescription_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in charactersDescription)
            {
                 entry.Serialize(writer);
                 charactersDescription_count++;
            }
            var charactersDescription_after = writer.Position;
            writer.Seek((int)charactersDescription_before);
            writer.WriteUShort((ushort)charactersDescription_count);
            writer.Seek((int)charactersDescription_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            var charactersDescription_ = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersDescription_[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 charactersDescription_[i].Deserialize(reader);
            }
            charactersDescription = charactersDescription_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(short) + charactersDescription.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}