

// Generated on 04/19/2016 10:17:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildModificationNameValidMessage : Message
    {
        public const uint Id = 6327;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string guildName;
        
        public GuildModificationNameValidMessage()
        {
        }
        
        public GuildModificationNameValidMessage(string guildName)
        {
            this.guildName = guildName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(guildName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            guildName = reader.ReadUTF();
        }
        
    }
    
}