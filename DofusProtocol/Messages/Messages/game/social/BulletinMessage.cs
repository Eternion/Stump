

// Generated on 10/30/2016 16:20:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BulletinMessage : SocialNoticeMessage
    {
        public const uint Id = 6695;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int lastNotifiedTimestamp;
        
        public BulletinMessage()
        {
        }
        
        public BulletinMessage(string content, int timestamp, long memberId, string memberName, int lastNotifiedTimestamp)
         : base(content, timestamp, memberId, memberName)
        {
            this.lastNotifiedTimestamp = lastNotifiedTimestamp;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(lastNotifiedTimestamp);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            lastNotifiedTimestamp = reader.ReadInt();
            if (lastNotifiedTimestamp < 0)
                throw new Exception("Forbidden value on lastNotifiedTimestamp = " + lastNotifiedTimestamp + ", it doesn't respect the following condition : lastNotifiedTimestamp < 0");
        }
        
    }
    
}