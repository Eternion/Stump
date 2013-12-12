

// Generated on 12/12/2013 16:56:56
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
        
        public GameFightSpectateMessage()
        {
        }
        
        public GameFightSpectateMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, short gameTurn)
        {
            this.effects = effects;
            this.marks = marks;
            this.gameTurn = gameTurn;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)marks.Count());
            foreach (var entry in marks)
            {
                 entry.Serialize(writer);
            }
            writer.WriteShort(gameTurn);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            effects = new Types.FightDispellableEffectExtendedInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.FightDispellableEffectExtendedInformations[])[i] = new Types.FightDispellableEffectExtendedInformations();
                 (effects as Types.FightDispellableEffectExtendedInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            marks = new Types.GameActionMark[limit];
            for (int i = 0; i < limit; i++)
            {
                 (marks as Types.GameActionMark[])[i] = new Types.GameActionMark();
                 (marks as Types.GameActionMark[])[i].Deserialize(reader);
            }
            gameTurn = reader.ReadShort();
            if (gameTurn < 0)
                throw new Exception("Forbidden value on gameTurn = " + gameTurn + ", it doesn't respect the following condition : gameTurn < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + effects.Sum(x => x.GetSerializationSize()) + sizeof(short) + marks.Sum(x => x.GetSerializationSize()) + sizeof(short);
        }
        
    }
    
}