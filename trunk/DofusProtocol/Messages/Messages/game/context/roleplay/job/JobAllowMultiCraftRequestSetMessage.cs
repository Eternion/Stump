
// Generated on 03/25/2013 19:24:11
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobAllowMultiCraftRequestSetMessage : Message
    {
        public const uint Id = 5749;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enabled;
        
        public JobAllowMultiCraftRequestSetMessage()
        {
        }
        
        public JobAllowMultiCraftRequestSetMessage(bool enabled)
        {
            this.enabled = enabled;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enabled = reader.ReadBoolean();
        }
        
    }
    
}