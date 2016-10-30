

// Generated on 10/30/2016 16:20:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FocusedExchangeReadyMessage : ExchangeReadyMessage
    {
        public const uint Id = 6701;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int focusActionId;
        
        public FocusedExchangeReadyMessage()
        {
        }
        
        public FocusedExchangeReadyMessage(bool ready, short step, int focusActionId)
         : base(ready, step)
        {
            this.focusActionId = focusActionId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(focusActionId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            focusActionId = reader.ReadVarInt();
            if (focusActionId < 0)
                throw new Exception("Forbidden value on focusActionId = " + focusActionId + ", it doesn't respect the following condition : focusActionId < 0");
        }
        
    }
    
}