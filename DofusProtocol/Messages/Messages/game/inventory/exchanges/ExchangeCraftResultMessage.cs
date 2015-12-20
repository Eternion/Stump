

// Generated on 12/20/2015 16:37:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftResultMessage : Message
    {
        public const uint Id = 5790;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte craftResult;
        
        public ExchangeCraftResultMessage()
        {
        }
        
        public ExchangeCraftResultMessage(sbyte craftResult)
        {
            this.craftResult = craftResult;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(craftResult);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            craftResult = reader.ReadSByte();
            if (craftResult < 0)
                throw new Exception("Forbidden value on craftResult = " + craftResult + ", it doesn't respect the following condition : craftResult < 0");
        }
        
    }
    
}