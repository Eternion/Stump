

// Generated on 09/01/2015 10:48:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkRecycleTradeMessage : Message
    {
        public const uint Id = 6600;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short percentToPrism;
        public short percentToPlayer;
        
        public ExchangeStartOkRecycleTradeMessage()
        {
        }
        
        public ExchangeStartOkRecycleTradeMessage(short percentToPrism, short percentToPlayer)
        {
            this.percentToPrism = percentToPrism;
            this.percentToPlayer = percentToPlayer;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(percentToPrism);
            writer.WriteShort(percentToPlayer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            percentToPrism = reader.ReadShort();
            if (percentToPrism < 0)
                throw new Exception("Forbidden value on percentToPrism = " + percentToPrism + ", it doesn't respect the following condition : percentToPrism < 0");
            percentToPlayer = reader.ReadShort();
            if (percentToPlayer < 0)
                throw new Exception("Forbidden value on percentToPlayer = " + percentToPlayer + ", it doesn't respect the following condition : percentToPlayer < 0");
        }
        
    }
    
}