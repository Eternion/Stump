

// Generated on 02/02/2016 14:14:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class FriendGuildSetWarnOnAchievementCompleteMessage : Message
    {
        public const uint Id = 6382;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enable;
        
        public FriendGuildSetWarnOnAchievementCompleteMessage()
        {
        }
        
        public FriendGuildSetWarnOnAchievementCompleteMessage(bool enable)
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