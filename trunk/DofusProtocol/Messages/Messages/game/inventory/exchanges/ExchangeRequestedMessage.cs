
// Generated on 03/25/2013 19:24:20
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeRequestedMessage : Message
    {
        public const uint Id = 5522;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte exchangeType;
        
        public ExchangeRequestedMessage()
        {
        }
        
        public ExchangeRequestedMessage(sbyte exchangeType)
        {
            this.exchangeType = exchangeType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(exchangeType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            exchangeType = reader.ReadSByte();
        }
        
    }
    
}