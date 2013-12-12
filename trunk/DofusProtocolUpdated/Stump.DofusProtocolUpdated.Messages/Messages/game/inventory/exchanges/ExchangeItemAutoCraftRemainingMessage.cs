

// Generated on 12/12/2013 16:57:15
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
            writer.WriteInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadInt();
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}