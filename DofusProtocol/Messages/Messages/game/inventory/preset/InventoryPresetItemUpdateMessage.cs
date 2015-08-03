

// Generated on 08/04/2015 00:37:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetItemUpdateMessage : Message
    {
        public const uint Id = 6168;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public Types.PresetItem presetItem;
        
        public InventoryPresetItemUpdateMessage()
        {
        }
        
        public InventoryPresetItemUpdateMessage(sbyte presetId, Types.PresetItem presetItem)
        {
            this.presetId = presetId;
            this.presetItem = presetItem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            presetItem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            presetItem = new Types.PresetItem();
            presetItem.Deserialize(reader);
        }
        
    }
    
}