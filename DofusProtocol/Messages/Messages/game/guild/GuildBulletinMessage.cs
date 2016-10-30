

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildBulletinMessage : BulletinMessage
    {
        public const uint Id = 6689;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GuildBulletinMessage()
        {
        }
        
        public GuildBulletinMessage(string content, int timestamp, long memberId, string memberName, int lastNotifiedTimestamp)
         : base(content, timestamp, memberId, memberName, lastNotifiedTimestamp)
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