

// Generated on 10/28/2014 16:36:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceChangeGuildRightsMessage : Message
    {
        public const uint Id = 6426;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int guildId;
        public sbyte rights;
        
        public AllianceChangeGuildRightsMessage()
        {
        }
        
        public AllianceChangeGuildRightsMessage(int guildId, sbyte rights)
        {
            this.guildId = guildId;
            this.rights = rights;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(guildId);
            writer.WriteSByte(rights);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            rights = reader.ReadSByte();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}