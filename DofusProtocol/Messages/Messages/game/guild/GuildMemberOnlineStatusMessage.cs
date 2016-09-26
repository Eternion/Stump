

// Generated on 09/26/2016 01:50:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildMemberOnlineStatusMessage : Message
    {
        public const uint Id = 6061;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long memberId;
        public bool online;
        
        public GuildMemberOnlineStatusMessage()
        {
        }
        
        public GuildMemberOnlineStatusMessage(long memberId, bool online)
        {
            this.memberId = memberId;
            this.online = online;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(memberId);
            writer.WriteBoolean(online);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            memberId = reader.ReadVarLong();
            if (memberId < 0 || memberId > 9007199254740990)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0 || memberId > 9007199254740990");
            online = reader.ReadBoolean();
        }
        
    }
    
}