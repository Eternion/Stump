

// Generated on 12/20/2015 16:37:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeItemAutoCraftStopedMessage : Message
    {
        public const uint Id = 5810;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public ExchangeItemAutoCraftStopedMessage()
        {
        }
        
        public ExchangeItemAutoCraftStopedMessage(sbyte reason)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reason = reader.ReadSByte();
        }
        
    }
    
}