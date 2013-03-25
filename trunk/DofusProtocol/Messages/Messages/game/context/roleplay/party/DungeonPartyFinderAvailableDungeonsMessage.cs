
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderAvailableDungeonsMessage : Message
    {
        public const uint Id = 6242;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> dungeonIds;
        
        public DungeonPartyFinderAvailableDungeonsMessage()
        {
        }
        
        public DungeonPartyFinderAvailableDungeonsMessage(IEnumerable<short> dungeonIds)
        {
            this.dungeonIds = dungeonIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)dungeonIds.Count());
            foreach (var entry in dungeonIds)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            dungeonIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dungeonIds as short[])[i] = reader.ReadShort();
            }
        }
        
    }
    
}