
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NpcDialogCreationMessage : Message
    {
        public const uint Id = 5618;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int mapId;
        public int npcId;
        
        public NpcDialogCreationMessage()
        {
        }
        
        public NpcDialogCreationMessage(int mapId, int npcId)
        {
            this.mapId = mapId;
            this.npcId = npcId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(mapId);
            writer.WriteInt(npcId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mapId = reader.ReadInt();
            npcId = reader.ReadInt();
        }
        
    }
    
}