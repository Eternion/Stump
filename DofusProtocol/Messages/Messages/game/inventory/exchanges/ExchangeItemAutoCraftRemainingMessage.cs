

// Generated on 04/24/2015 03:38:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeItemAutoCraftRemainingMessage : Message
    {
        public const uint Id = 6015;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeItemAutoCraftRemainingMessage()
        {
        }
        
        public ExchangeItemAutoCraftRemainingMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadVarInt();
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
        }
        
    }
    
}