

// Generated on 03/06/2014 18:50:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildHouseTeleportRequestMessage : Message
    {
        public const uint Id = 5712;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        
        public GuildHouseTeleportRequestMessage()
        {
        }
        
        public GuildHouseTeleportRequestMessage(int houseId)
        {
            this.houseId = houseId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(houseId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}