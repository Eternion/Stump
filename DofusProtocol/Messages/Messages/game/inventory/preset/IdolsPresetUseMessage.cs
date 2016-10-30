

// Generated on 10/30/2016 16:20:47
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolsPresetUseMessage : Message
    {
        public const uint Id = 6615;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public bool party;
        
        public IdolsPresetUseMessage()
        {
        }
        
        public IdolsPresetUseMessage(sbyte presetId, bool party)
        {
            this.presetId = presetId;
            this.party = party;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteBoolean(party);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            party = reader.ReadBoolean();
        }
        
    }
    
}