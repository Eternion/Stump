

// Generated on 09/01/2014 15:51:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServersListMessage : Message
    {
        public const uint Id = 30;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GameServerInformations> servers;
        
        public ServersListMessage()
        {
        }
        
        public ServersListMessage(IEnumerable<Types.GameServerInformations> servers)
        {
            this.servers = servers;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var servers_before = writer.Position;
            var servers_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in servers)
            {
                 entry.Serialize(writer);
                 servers_count++;
            }
            var servers_after = writer.Position;
            writer.Seek((int)servers_before);
            writer.WriteUShort((ushort)servers_count);
            writer.Seek((int)servers_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var servers_ = new Types.GameServerInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 servers_[i] = new Types.GameServerInformations();
                 servers_[i].Deserialize(reader);
            }
            servers = servers_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + servers.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}