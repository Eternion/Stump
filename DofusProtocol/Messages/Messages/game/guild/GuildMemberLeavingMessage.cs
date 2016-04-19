

// Generated on 04/19/2016 10:17:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMemberLeavingMessage : Message
    {
        public const uint Id = 5923;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool kicked;
        public long memberId;
        
        public GuildMemberLeavingMessage()
        {
        }
        
        public GuildMemberLeavingMessage(bool kicked, long memberId)
        {
            this.kicked = kicked;
            this.memberId = memberId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(kicked);
            writer.WriteVarLong(memberId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kicked = reader.ReadBoolean();
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
        }
        
    }
    
}