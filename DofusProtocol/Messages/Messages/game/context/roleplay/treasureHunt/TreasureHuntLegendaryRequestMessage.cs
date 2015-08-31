

// Generated on 08/04/2015 13:25:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TreasureHuntLegendaryRequestMessage : Message
    {
        public const uint Id = 6499;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short legendaryId;
        
        public TreasureHuntLegendaryRequestMessage()
        {
        }
        
        public TreasureHuntLegendaryRequestMessage(short legendaryId)
        {
            this.legendaryId = legendaryId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(legendaryId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            legendaryId = reader.ReadVarShort();
            if (legendaryId < 0)
                throw new Exception("Forbidden value on legendaryId = " + legendaryId + ", it doesn't respect the following condition : legendaryId < 0");
        }
        
    }
    
}