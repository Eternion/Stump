

// Generated on 09/26/2016 01:50:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public short genId;
        
        public ExchangeBidHousePriceMessage()
        {
        }
        
        public ExchangeBidHousePriceMessage(short genId)
        {
            this.genId = genId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(genId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            genId = reader.ReadVarShort();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
        }
        
    }
    
}