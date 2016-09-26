

// Generated on 09/26/2016 01:49:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceMotdMessage : SocialNoticeMessage
    {
        public const uint Id = 6685;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceMotdMessage()
        {
        }
        
        public AllianceMotdMessage(string content, int timestamp, long memberId, string memberName)
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