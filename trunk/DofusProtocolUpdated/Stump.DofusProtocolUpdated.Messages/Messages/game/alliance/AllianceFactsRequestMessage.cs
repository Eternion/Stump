

// Generated on 03/05/2014 20:34:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceFactsRequestMessage : Message
    {
        public const uint Id = 6409;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int allianceId;
        
        public AllianceFactsRequestMessage()
        {
        }
        
        public AllianceFactsRequestMessage(int allianceId)
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