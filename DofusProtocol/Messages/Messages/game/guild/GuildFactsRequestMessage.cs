

// Generated on 02/11/2015 10:20:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFactsRequestMessage : Message
    {
        public const uint Id = 6404;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int guildId;
        
        public GuildFactsRequestMessage()
        {
        }
        
        public GuildFactsRequestMessage(int guildId)
        {
            this.guildId = guildId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(guildId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildId = reader.ReadVarInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
        }
        
    }
    
}