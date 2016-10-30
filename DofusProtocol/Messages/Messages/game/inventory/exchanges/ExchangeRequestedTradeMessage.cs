

// Generated on 10/30/2016 16:20:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeRequestedTradeMessage : ExchangeRequestedMessage
    {
        public const uint Id = 5523;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long source;
        public long target;
        
        public ExchangeRequestedTradeMessage()
        {
        }
        
        public ExchangeRequestedTradeMessage(sbyte exchangeType, long source, long target)
         : base(exchangeType)
        {
            this.source = source;
            this.target = target;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(source);
            writer.WriteVarLong(target);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            source = reader.ReadVarLong();
            if (source < 0 || source > 9007199254740990)
                throw new Exception("Forbidden value on source = " + source + ", it doesn't respect the following condition : source < 0 || source > 9007199254740990");
            target = reader.ReadVarLong();
            if (target < 0 || target > 9007199254740990)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0 || target > 9007199254740990");
        }
        
    }
    
}