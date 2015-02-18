

// Generated on 02/18/2015 10:46:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildModificationEmblemValidMessage : Message
    {
        public const uint Id = 6328;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GuildEmblem guildEmblem;
        
        public GuildModificationEmblemValidMessage()
        {
        }
        
        public GuildModificationEmblemValidMessage(Types.GuildEmblem guildEmblem)
        {
            this.guildEmblem = guildEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            guildEmblem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
        
    }
    
}