

// Generated on 12/20/2015 16:37:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangePlayerRequestMessage : ExchangeRequestMessage
    {
        public const uint Id = 5773;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long target;
        
        public ExchangePlayerRequestMessage()
        {
        }
        
        public ExchangePlayerRequestMessage(sbyte exchangeType, long target)
         : base(exchangeType)
        {
            this.target = target;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(target);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            target = reader.ReadVarLong();
            if (target < 0 || target > 9.007199254740992E15)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0 || target > 9.007199254740992E15");
        }
        
    }
    
}