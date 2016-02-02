

// Generated on 02/02/2016 14:14:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildModificationValidMessage : Message
    {
        public const uint Id = 6323;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string guildName;
        public Types.GuildEmblem guildEmblem;
        
        public GuildModificationValidMessage()
        {
        }
        
        public GuildModificationValidMessage(string guildName, Types.GuildEmblem guildEmblem)
        {
            this.guildName = guildName;
            this.guildEmblem = guildEmblem;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(guildName);
            guildEmblem.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildName = reader.ReadUTF();
            guildEmblem = new Types.GuildEmblem();
            guildEmblem.Deserialize(reader);
        }
        
    }
    
}