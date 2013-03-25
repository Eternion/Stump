
// Generated on 03/25/2013 19:23:58
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)servers.Count());
            foreach (var entry in servers)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            servers = new Types.GameServerInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (servers as Types.GameServerInformations[])[i] = new Types.GameServerInformations();
                 (servers as Types.GameServerInformations[])[i].Deserialize(reader);
            }
        }
        
    }
    
}