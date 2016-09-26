

// Generated on 09/26/2016 01:49:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceBulletinMessage : BulletinMessage
    {
        public const uint Id = 6690;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceBulletinMessage()
        {
        }
        
        public AllianceBulletinMessage(string content, int timestamp, long memberId, string memberName, int lastNotifiedTimestamp)
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