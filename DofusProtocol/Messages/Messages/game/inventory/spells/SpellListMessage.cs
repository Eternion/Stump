

// Generated on 02/11/2015 10:20:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SpellListMessage : Message
    {
        public const uint Id = 1200;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool spellPrevisualization;
        public IEnumerable<Types.SpellItem> spells;
        
        public SpellListMessage()
        {
        }
        
        public SpellListMessage(bool spellPrevisualization, IEnumerable<Types.SpellItem> spells)
        {
            this.spellPrevisualization = spellPrevisualization;
            this.spells = spells;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(spellPrevisualization);
            var spells_before = writer.Position;
            var spells_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in spells)
            {
                 entry.Serialize(writer);
                 spells_count++;
            }
            var spells_after = writer.Position;
            writer.Seek((int)spells_before);
            writer.WriteUShort((ushort)spells_count);
            writer.Seek((int)spells_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            spellPrevisualization = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var spells_ = new Types.SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 spells_[i] = new Types.SpellItem();
                 spells_[i].Deserialize(reader);
            }
            spells = spells_;
        }
        
    }
    
}