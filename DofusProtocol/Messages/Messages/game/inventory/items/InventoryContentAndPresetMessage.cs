

// Generated on 08/04/2015 13:25:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryContentAndPresetMessage : InventoryContentMessage
    {
        public const uint Id = 6162;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.Preset> presets;
        
        public InventoryContentAndPresetMessage()
        {
        }
        
        public InventoryContentAndPresetMessage(IEnumerable<Types.ObjectItem> objects, int kamas, IEnumerable<Types.Preset> presets)
         : base(objects, kamas)
        {
            this.presets = presets;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var presets_before = writer.Position;
            var presets_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in presets)
            {
                 entry.Serialize(writer);
                 presets_count++;
            }
            var presets_after = writer.Position;
            writer.Seek((int)presets_before);
            writer.WriteUShort((ushort)presets_count);
            writer.Seek((int)presets_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var presets_ = new Types.Preset[limit];
            for (int i = 0; i < limit; i++)
            {
                 presets_[i] = new Types.Preset();
                 presets_[i].Deserialize(reader);
            }
            presets = presets_;
        }
        
    }
    
}