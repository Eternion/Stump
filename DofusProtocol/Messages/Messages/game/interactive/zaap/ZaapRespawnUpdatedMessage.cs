

// Generated on 02/02/2016 14:14:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ZaapRespawnUpdatedMessage : Message
    {
        public const uint Id = 6571;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        
        public ZaapRespawnUpdatedMessage()
        {
        }
        
        public ZaapRespawnUpdatedMessage(int mapId)
        {
            this.mapId = mapId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
        }
        
    }
    
}