

// Generated on 02/02/2016 14:14:29
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
            var guilds_before = writer.Position;
            var guilds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
                 guilds_count++;
            }
            var guilds_after = writer.Position;
            writer.Seek((int)guilds_before);
            writer.WriteUShort((ushort)guilds_count);
            writer.Seek((int)guilds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var guilds_ = new Types.GuildInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guilds_[i] = new Types.GuildInformations();
                 guilds_[i].Deserialize(reader);
            }
            guilds = guilds_;
        }
        
    }
    
}