

// Generated on 10/30/2016 16:20:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkNpcTradeMessage : Message
    {
        public const uint Id = 5785;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double npcId;
        
        public ExchangeStartOkNpcTradeMessage()
        {
        }
        
        public ExchangeStartOkNpcTradeMessage(double npcId)
        {
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            npcId = reader.ReadDouble();
            if (npcId < -9007199254740990 || npcId > 9007199254740990)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < -9007199254740990 || npcId > 9007199254740990");
        }
        
    }
    
}