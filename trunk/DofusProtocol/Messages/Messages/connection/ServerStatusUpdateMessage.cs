
// Generated on 03/25/2013 19:23:58
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerStatusUpdateMessage : Message
    {
        public const uint Id = 50;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.GameServerInformations server;
        
        public ServerStatusUpdateMessage()
        {
        }
        
        public ServerStatusUpdateMessage(Types.GameServerInformations server)
        {
            this.server = server;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            server.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            server = new Types.GameServerInformations();
            server.Deserialize(reader);
        }
        
    }
    
}