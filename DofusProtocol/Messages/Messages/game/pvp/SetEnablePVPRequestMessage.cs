

// Generated on 02/19/2015 12:09:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SetEnablePVPRequestMessage : Message
    {
        public const uint Id = 1810;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public SetEnablePVPRequestMessage()
        {
        }
        
        public SetEnablePVPRequestMessage(bool enable)
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