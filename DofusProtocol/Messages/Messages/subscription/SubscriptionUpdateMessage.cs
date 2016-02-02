

// Generated on 02/02/2016 14:14:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SubscriptionUpdateMessage : Message
    {
        public const uint Id = 6616;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double timestamp;
        
        public SubscriptionUpdateMessage()
        {
        }
        
        public SubscriptionUpdateMessage(double timestamp)
        {
            this.timestamp = timestamp;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(timestamp);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            timestamp = reader.ReadDouble();
            if (timestamp < -9.007199254740992E15 || timestamp > 9.007199254740992E15)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < -9.007199254740992E15 || timestamp > 9.007199254740992E15");
        }
        
    }
    
}