

// Generated on 04/24/2015 03:37:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicTimeMessage : Message
    {
        public const uint Id = 175;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double timestamp;
        public short timezoneOffset;
        
        public BasicTimeMessage()
        {
        }
        
        public BasicTimeMessage(double timestamp, short timezoneOffset)
        {
            this.timestamp = timestamp;
            this.timezoneOffset = timezoneOffset;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(timestamp);
            writer.WriteShort(timezoneOffset);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            timestamp = reader.ReadDouble();
            if (timestamp < 0 || timestamp > 9.007199254740992E15)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0 || timestamp > 9.007199254740992E15");
            timezoneOffset = reader.ReadShort();
        }
        
    }
    
}