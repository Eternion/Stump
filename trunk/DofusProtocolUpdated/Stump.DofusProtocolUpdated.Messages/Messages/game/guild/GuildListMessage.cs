

// Generated on 12/12/2013 16:57:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildListMessage : Message
    {
        public const uint Id = 6413;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GuildInformations> guilds;
        
        public GuildListMessage()
        {
        }
        
        public GuildListMessage(IEnumerable<Types.GuildInformations> guilds)
        {
            this.guilds = guilds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInformations[])[i] = new Types.GuildInformations();
                 (guilds as Types.GuildInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + guilds.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}