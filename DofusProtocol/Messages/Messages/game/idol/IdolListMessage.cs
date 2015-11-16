

// Generated on 11/16/2015 14:26:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolListMessage : Message
    {
        public const uint Id = 6585;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> chosenIdols;
        public IEnumerable<short> partyChosenIdols;
        public IEnumerable<Types.PartyIdol> partyIdols;
        
        public IdolListMessage()
        {
        }
        
        public IdolListMessage(IEnumerable<short> chosenIdols, IEnumerable<short> partyChosenIdols, IEnumerable<Types.PartyIdol> partyIdols)
        {
            this.chosenIdols = chosenIdols;
            this.partyChosenIdols = partyChosenIdols;
            this.partyIdols = partyIdols;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var chosenIdols_before = writer.Position;
            var chosenIdols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in chosenIdols)
            {
                 writer.WriteVarShort(entry);
                 chosenIdols_count++;
            }
            var chosenIdols_after = writer.Position;
            writer.Seek((int)chosenIdols_before);
            writer.WriteUShort((ushort)chosenIdols_count);
            writer.Seek((int)chosenIdols_after);

            var partyChosenIdols_before = writer.Position;
            var partyChosenIdols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in partyChosenIdols)
            {
                 writer.WriteVarShort(entry);
                 partyChosenIdols_count++;
            }
            var partyChosenIdols_after = writer.Position;
            writer.Seek((int)partyChosenIdols_before);
            writer.WriteUShort((ushort)partyChosenIdols_count);
            writer.Seek((int)partyChosenIdols_after);

            var partyIdols_before = writer.Position;
            var partyIdols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in partyIdols)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 partyIdols_count++;
            }
            var partyIdols_after = writer.Position;
            writer.Seek((int)partyIdols_before);
            writer.WriteUShort((ushort)partyIdols_count);
            writer.Seek((int)partyIdols_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var chosenIdols_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 chosenIdols_[i] = reader.ReadVarShort();
            }
            chosenIdols = chosenIdols_;
            limit = reader.ReadUShort();
            var partyChosenIdols_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 partyChosenIdols_[i] = reader.ReadVarShort();
            }
            partyChosenIdols = partyChosenIdols_;
            limit = reader.ReadUShort();
            var partyIdols_ = new Types.PartyIdol[limit];
            for (int i = 0; i < limit; i++)
            {
                 partyIdols_[i] = Types.ProtocolTypeManager.GetInstance<Types.PartyIdol>(reader.ReadShort());
                 partyIdols_[i].Deserialize(reader);
            }
            partyIdols = partyIdols_;
        }
        
    }
    
}