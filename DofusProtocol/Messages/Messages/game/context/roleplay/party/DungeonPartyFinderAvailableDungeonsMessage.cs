

// Generated on 04/19/2016 10:17:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var dungeonIds_before = writer.Position;
            var dungeonIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dungeonIds)
            {
                 writer.WriteVarShort(entry);
                 dungeonIds_count++;
            }
            var dungeonIds_after = writer.Position;
            writer.Seek((int)dungeonIds_before);
            writer.WriteUShort((ushort)dungeonIds_count);
            writer.Seek((int)dungeonIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var dungeonIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 dungeonIds_[i] = reader.ReadVarShort();
            }
            dungeonIds = dungeonIds_;
        }
        
    }
    
}