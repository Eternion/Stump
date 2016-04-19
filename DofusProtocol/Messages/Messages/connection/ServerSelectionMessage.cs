

// Generated on 04/19/2016 10:17:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ServerSelectionMessage : Message
    {
        public const uint Id = 40;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short serverId;
        
        public ServerSelectionMessage()
        {
        }
        
        public ServerSelectionMessage(short serverId)
        {
            this.serverId = serverId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(serverId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            serverId = reader.ReadVarShort();
            if (serverId < 0)
                throw new Exception("Forbidden value on serverId = " + serverId + ", it doesn't respect the following condition : serverId < 0");
        }
        
    }
    
}