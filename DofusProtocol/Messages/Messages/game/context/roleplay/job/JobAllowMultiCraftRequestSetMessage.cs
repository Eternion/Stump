

// Generated on 10/27/2014 19:57:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public override int GetSerializationSize()
        {
            return sizeof(bool);
        }
        
    }
    
}