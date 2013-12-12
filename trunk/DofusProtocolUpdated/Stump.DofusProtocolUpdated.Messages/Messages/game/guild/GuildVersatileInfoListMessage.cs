

// Generated on 12/12/2013 16:57:11
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
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            guilds = new Types.GuildVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildVersatileInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GuildVersatileInformations>(reader.ReadShort());
                 (guilds as Types.GuildVersatileInformations[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + guilds.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}