

// Generated on 03/02/2014 20:42:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightDefendersStateMessage : Message
    {
        public const uint Id = 5899;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double fightId;
        public IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> mainFighters;
        public IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> reserveFighters;
        
        public PrismFightDefendersStateMessage()
        {
        }
        
        public PrismFightDefendersStateMessage(double fightId, IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> mainFighters, IEnumerable<Types.CharacterMinimalPlusLookAndGradeInformations> reserveFighters)
        {
            this.fightId = fightId;
            this.mainFighters = mainFighters;
            this.reserveFighters = reserveFighters;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(fightId);
            var mainFighters_before = writer.Position;
            var mainFighters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in mainFighters)
            {
                 entry.Serialize(writer);
                 mainFighters_count++;
            }
            var mainFighters_after = writer.Position;
            writer.Seek((int)mainFighters_before);
            writer.WriteUShort((ushort)mainFighters_count);
            writer.Seek((int)mainFighters_after);

            var reserveFighters_before = writer.Position;
            var reserveFighters_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in reserveFighters)
            {
                 entry.Serialize(writer);
                 reserveFighters_count++;
            }
            var reserveFighters_after = writer.Position;
            writer.Seek((int)reserveFighters_before);
            writer.WriteUShort((ushort)reserveFighters_count);
            writer.Seek((int)reserveFighters_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            var mainFighters_ = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 mainFighters_[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 mainFighters_[i].Deserialize(reader);
            }
            mainFighters = mainFighters_;
            limit = reader.ReadUShort();
            var reserveFighters_ = new Types.CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 reserveFighters_[i] = new Types.CharacterMinimalPlusLookAndGradeInformations();
                 reserveFighters_[i].Deserialize(reader);
            }
            reserveFighters = reserveFighters_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(short) + mainFighters.Sum(x => x.GetSerializationSize()) + sizeof(short) + reserveFighters.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}