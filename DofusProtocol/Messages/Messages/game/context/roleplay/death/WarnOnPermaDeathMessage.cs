

// Generated on 02/11/2015 10:20:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class WarnOnPermaDeathMessage : Message
    {
        public const uint Id = 6512;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public WarnOnPermaDeathMessage()
        {
        }
        
        public WarnOnPermaDeathMessage(bool enable)
        {
            this.enable = enable;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(enable);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            enable = reader.ReadBoolean();
        }
        
    }
    
}