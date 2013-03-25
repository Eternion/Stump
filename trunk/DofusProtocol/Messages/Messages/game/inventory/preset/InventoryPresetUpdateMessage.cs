
// Generated on 03/25/2013 19:24:23
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetUpdateMessage : Message
    {
        public const uint Id = 6171;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.Preset preset;
        
        public InventoryPresetUpdateMessage()
        {
        }
        
        public InventoryPresetUpdateMessage(Types.Preset preset)
        {
            this.preset = preset;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            preset.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            preset = new Types.Preset();
            preset.Deserialize(reader);
        }
        
    }
    
}