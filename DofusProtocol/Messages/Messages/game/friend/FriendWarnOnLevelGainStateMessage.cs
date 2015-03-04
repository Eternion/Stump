

// Generated on 02/19/2015 12:09:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendWarnOnLevelGainStateMessage : Message
    {
        public const uint Id = 6078;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public FriendWarnOnLevelGainStateMessage()
        {
        }
        
        public FriendWarnOnLevelGainStateMessage(bool enable)
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