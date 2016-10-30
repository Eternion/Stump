

// Generated on 10/30/2016 16:20:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightSpectateMessage : Message
    {
        public const uint Id = 6069;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.FightDispellableEffectExtendedInformations> effects;
        public IEnumerable<Types.GameActionMark> marks;
        public short gameTurn;
        public int fightStart;
        public IEnumerable<Types.Idol> idols;
        
        public GameFightSpectateMessage()
        {
        }
        
        public GameFightSpectateMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, short gameTurn, int fightStart, IEnumerable<Types.Idol> idols)
        {
            this.effects = effects;
            this.marks = marks;
            this.gameTurn = gameTurn;
            this.fightStart = fightStart;
            this.idols = idols;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var effects_before = writer.Position;
            var effects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in effects)
            {
                 entry.Serialize(writer);
                 effects_count++;
            }
            var effects_after = writer.Position;
            writer.Seek((int)effects_before);
            writer.WriteUShort((ushort)effects_count);
            writer.Seek((int)effects_after);

            var marks_before = writer.Position;
            var marks_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in marks)
            {
                 entry.Serialize(writer);
                 marks_count++;
            }
            var marks_after = writer.Position;
            writer.Seek((int)marks_before);
            writer.WriteUShort((ushort)marks_count);
            writer.Seek((int)marks_after);

            writer.WriteVarShort(gameTurn);
            writer.WriteInt(fightStart);
            var idols_before = writer.Position;
            var idols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in idols)
            {
                 entry.Serialize(writer);
                 idols_count++;
            }
            var idols_after = writer.Position;
            writer.Seek((int)idols_before);
            writer.WriteUShort((ushort)idols_count);
            writer.Seek((int)idols_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var effects_ = new Types.FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects_[i] = new Types.FightDispellableEffectExtendedInformations();
                 effects_[i].Deserialize(reader);
            }
            effects = effects_;
            limit = reader.ReadUShort();
            var marks_ = new Types.GameActionMark[limit];
            for (int i = 0; i < limit; i++)
            {
                 marks_[i] = new Types.GameActionMark();
                 marks_[i].Deserialize(reader);
            }
            marks = marks_;
            gameTurn = reader.ReadVarShort();
            if (gameTurn < 0)
                throw new Exception("Forbidden value on gameTurn = " + gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
            fightStart = reader.ReadInt();
            if (fightStart < 0)
                throw new Exception("Forbidden value on fightStart = " + fightStart + ", it doesn't respect the following condition : fightStart < 0");
            limit = reader.ReadUShort();
            var idols_ = new Types.Idol[limit];
            for (int i = 0; i < limit; i++)
            {
                 idols_[i] = new Types.Idol();
                 idols_[i].Deserialize(reader);
            }
            idols = idols_;
        }
        
    }
    
}