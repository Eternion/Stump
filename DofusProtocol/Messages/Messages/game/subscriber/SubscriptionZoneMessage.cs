

// Generated on 11/16/2015 14:26:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SubscriptionZoneMessage : Message
    {
        public const uint Id = 5573;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool active;
        
        public SubscriptionZoneMessage()
        {
        }
        
        public SubscriptionZoneMessage(bool active)
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