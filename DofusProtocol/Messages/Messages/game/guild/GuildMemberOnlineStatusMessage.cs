

// Generated on 10/26/2014 23:29:33
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
        
        public int memberId;
        public bool online;
        
        public GuildMemberOnlineStatusMessage()
        {
        }
        
        public GuildMemberOnlineStatusMessage(int memberId, bool online)
        {
            this.memberId = memberId;
            this.online = online;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(memberId);
            writer.WriteBoolean(online);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            memberId = reader.ReadInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            online = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(bool);
        }
        
    }
    
}