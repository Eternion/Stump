

// Generated on 12/29/2014 21:13:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildVersatileInfoListMessage : Message
    {
        public const uint Id = 6435;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GuildVersatileInformations> guilds;
        
        public GuildVersatileInfoListMessage()
        {
        }
        
        public GuildVersatileInfoListMessage(IEnumerable<Types.GuildVersatileInformations> guilds)
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
                 writer.WriteShort(entry.TypeId);
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
            var guilds_ = new Types.GuildVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 guilds_[i] = Types.ProtocolTypeManager.GetInstance<Types.GuildVersatileInformations>(reader.ReadShort());
                 guilds_[i].Deserialize(reader);
            }
            guilds = guilds_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + guilds.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}