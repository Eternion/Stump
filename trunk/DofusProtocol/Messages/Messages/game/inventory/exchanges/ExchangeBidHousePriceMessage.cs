
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHousePriceMessage : Message
    {
        public const uint Id = 5805;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int genId;
        
        public ExchangeBidHousePriceMessage()
        {
        }
        
        public ExchangeBidHousePriceMessage(int genId)
        {
            this.genId = genId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(genId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genId = reader.ReadInt();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
        }
        
    }
    
}