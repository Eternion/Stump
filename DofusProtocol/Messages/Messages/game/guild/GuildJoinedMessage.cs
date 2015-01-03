

// Generated on 12/29/2014 21:13:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildJoinedMessage : Message
    {
        public const uint Id = 5564;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInfo;
        public int memberRights;
        public bool enabled;
        
        public GuildJoinedMessage()
        {
        }
        
        public GuildJoinedMessage(Types.GuildInformations guildInfo, int memberRights, bool enabled)
        {
            this.guildInfo = guildInfo;
            this.memberRights = memberRights;
            this.enabled = enabled;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            guildInfo.Serialize(writer);
            writer.WriteInt(memberRights);
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
            memberRights = reader.ReadInt();
            if (memberRights < 0)
                throw new Exception("Forbidden value on memberRights = " + memberRights + ", it doesn't respect the following condition : memberRights < 0");
            enabled = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return guildInfo.GetSerializationSize() + sizeof(int) + sizeof(bool);
        }
        
    }
    
}