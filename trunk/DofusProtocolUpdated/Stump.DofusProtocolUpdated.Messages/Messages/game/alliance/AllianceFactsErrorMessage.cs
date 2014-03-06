

// Generated on 03/06/2014 18:50:03
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
            writer.WriteInt(allianceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            allianceId = reader.ReadInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}