

// Generated on 10/30/2016 16:20:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeIsReadyMessage : Message
    {
        public const uint Id = 5509;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double id;
        public bool ready;
        
        public ExchangeIsReadyMessage()
        {
        }
        
        public ExchangeIsReadyMessage(double id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteBoolean(ready);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadDouble();
            if (id < -9007199254740990 || id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            ready = reader.ReadBoolean();
        }
        
    }
    
}