
// Generated on 03/25/2013 19:24:09
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CurrentMapMessage : Message
    {
        public const uint Id = 220;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        public string mapKey;
        
        public CurrentMapMessage()
        {
        }
        
        public CurrentMapMessage(int mapId, string mapKey)
        {
            this.mapId = mapId;
            this.mapKey = mapKey;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteUTF(mapKey);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            if (mapId < 0)
                throw new Exception("Forbidden value on mapId = " + mapId + ", it doesn't respect the following condition : mapId < 0");
            mapKey = reader.ReadUTF();
        }
        
    }
    
}