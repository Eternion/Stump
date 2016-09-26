

// Generated on 09/26/2016 01:50:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolsPresetDeleteResultMessage : Message
    {
        public const uint Id = 6605;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte code;
        
        public IdolsPresetDeleteResultMessage()
        {
        }
        
        public IdolsPresetDeleteResultMessage(sbyte presetId, sbyte code)
        {
            this.presetId = presetId;
            this.code = code;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(code);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            code = reader.ReadSByte();
            if (code < 0)
                throw new Exception("Forbidden value on code = " + code + ", it doesn't respect the following condition : code < 0");
        }
        
    }
    
}