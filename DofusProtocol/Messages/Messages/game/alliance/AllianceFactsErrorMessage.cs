

// Generated on 10/30/2016 16:20:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceFactsErrorMessage : Message
    {
        public const uint Id = 6423;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int allianceId;
        
        public AllianceFactsErrorMessage()
        {
        }
        
        public AllianceFactsErrorMessage(int allianceId)
        {
            this.allianceId = allianceId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(allianceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allianceId = reader.ReadVarInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
    }
    
}