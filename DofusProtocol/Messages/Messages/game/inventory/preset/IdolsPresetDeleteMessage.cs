

// Generated on 09/01/2015 10:48:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolsPresetDeleteMessage : Message
    {
        public const uint Id = 6602;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        
        public IdolsPresetDeleteMessage()
        {
        }
        
        public IdolsPresetDeleteMessage(sbyte presetId)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
    }
    
}