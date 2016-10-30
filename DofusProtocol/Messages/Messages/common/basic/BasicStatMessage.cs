

// Generated on 10/30/2016 16:20:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicStatMessage : Message
    {
        public const uint Id = 6530;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double timeSpent;
        public short statId;
        
        public BasicStatMessage()
        {
        }
        
        public BasicStatMessage(double timeSpent, short statId)
        {
            this.timeSpent = timeSpent;
            this.statId = statId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(timeSpent);
            writer.WriteVarShort(statId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            timeSpent = reader.ReadDouble();
            if (timeSpent < 0 || timeSpent > 9007199254740990)
                throw new Exception("Forbidden value on timeSpent = " + timeSpent + ", it doesn't respect the following condition : timeSpent < 0 || timeSpent > 9007199254740990");
            statId = reader.ReadVarShort();
            if (statId < 0)
                throw new Exception("Forbidden value on statId = " + statId + ", it doesn't respect the following condition : statId < 0");
        }
        
    }
    
}