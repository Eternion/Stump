

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuestModeMessage : Message
    {
        public const uint Id = 6505;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool active;
        
        public GuestModeMessage()
        {
        }
        
        public GuestModeMessage(bool active)
        {
            this.active = active;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(active);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            active = reader.ReadBoolean();
        }
        
    }
    
}