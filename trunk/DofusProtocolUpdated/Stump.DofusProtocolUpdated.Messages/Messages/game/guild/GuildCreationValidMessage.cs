

// Generated on 03/05/2014 20:34:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildCreationValidMessage : Message
    {
        public const uint Id = 5546;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string guildName;
        public Types.GuildEmblem guildEmblem;
        
        public GuildCreationValidMessage()
        {
        }
        
        public GuildCreationValidMessage(string guildName, Types.GuildEmblem guildEmblem)
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
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(guildName) + guildEmblem.GetSerializationSize();
        }
        
    }
    
}