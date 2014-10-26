

// Generated on 10/26/2014 23:29:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DungeonKeyRingMessage : Message
    {
        public const uint Id = 6299;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> availables;
        public IEnumerable<short> unavailables;
        
        public DungeonKeyRingMessage()
        {
        }
        
        public DungeonKeyRingMessage(IEnumerable<short> availables, IEnumerable<short> unavailables)
        {
            this.availables = availables;
            this.unavailables = unavailables;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var availables_before = writer.Position;
            var availables_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in availables)
            {
                 writer.WriteShort(entry);
                 availables_count++;
            }
            var availables_after = writer.Position;
            writer.Seek((int)availables_before);
            writer.WriteUShort((ushort)availables_count);
            writer.Seek((int)availables_after);

            var unavailables_before = writer.Position;
            var unavailables_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in unavailables)
            {
                 writer.WriteShort(entry);
                 unavailables_count++;
            }
            var unavailables_after = writer.Position;
            writer.Seek((int)unavailables_before);
            writer.WriteUShort((ushort)unavailables_count);
            writer.Seek((int)unavailables_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var availables_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 availables_[i] = reader.ReadShort();
            }
            availables = availables_;
            limit = reader.ReadUShort();
            var unavailables_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 unavailables_[i] = reader.ReadShort();
            }
            unavailables = unavailables_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + availables.Sum(x => sizeof(short)) + sizeof(short) + unavailables.Sum(x => sizeof(short));
        }
        
    }
    
}