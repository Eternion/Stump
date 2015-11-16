

// Generated on 11/16/2015 14:26:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolsPresetUpdateMessage : Message
    {
        public const uint Id = 6606;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.IdolsPreset idolsPreset;
        
        public IdolsPresetUpdateMessage()
        {
        }
        
        public IdolsPresetUpdateMessage(Types.IdolsPreset idolsPreset)
        {
            this.idolsPreset = idolsPreset;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            idolsPreset.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            idolsPreset = new Types.IdolsPreset();
            idolsPreset.Deserialize(reader);
        }
        
    }
    
}