

// Generated on 10/30/2016 16:20:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonPartyFinderRegisterRequestMessage : Message
    {
        public const uint Id = 6249;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> dungeonIds;
        
        public DungeonPartyFinderRegisterRequestMessage()
        {
        }
        
        public DungeonPartyFinderRegisterRequestMessage(IEnumerable<short> dungeonIds)
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