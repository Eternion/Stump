

// Generated on 10/30/2016 16:20:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMotdMessage : SocialNoticeMessage
    {
        public const uint Id = 6590;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GuildMotdMessage()
        {
        }
        
        public GuildMotdMessage(string content, int timestamp, long memberId, string memberName)
         : base(content, timestamp, memberId, memberName)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}