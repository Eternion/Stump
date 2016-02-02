

// Generated on 02/02/2016 14:14:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeReadyMessage : Message
    {
        public const uint Id = 5511;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool ready;
        public short step;
        
        public ExchangeReadyMessage()
        {
        }
        
        public ExchangeReadyMessage(bool ready, short step)
        {
            this.ready = ready;
            this.step = step;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(ready);
            writer.WriteVarShort(step);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ready = reader.ReadBoolean();
            step = reader.ReadVarShort();
            if (step < 0)
                throw new Exception("Forbidden value on step = " + step + ", it doesn't respect the following condition : step < 0");
        }
        
    }
    
}