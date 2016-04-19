

// Generated on 04/19/2016 10:17:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMotdMessage : Message
    {
        public const uint Id = 6590;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string content;
        public int timestamp;
        public long memberId;
        public string memberName;
        
        public GuildMotdMessage()
        {
        }
        
        public GuildMotdMessage(string content, int timestamp, long memberId, string memberName)
        {
            this.content = content;
            this.timestamp = timestamp;
            this.memberId = memberId;
            this.memberName = memberName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(content);
            writer.WriteInt(timestamp);
            writer.WriteVarLong(memberId);
            writer.WriteUTF(memberName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            content = reader.ReadUTF();
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            memberName = reader.ReadUTF();
        }
        
    }
    
}