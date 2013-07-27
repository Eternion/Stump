

// Generated on 07/26/2013 22:50:54
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)availables.Count());
            foreach (var entry in availables)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)unavailables.Count());
            foreach (var entry in unavailables)
            {
                 writer.WriteShort(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            availables = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (availables as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            unavailables = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (unavailables as short[])[i] = reader.ReadShort();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + availables.Sum(x => sizeof(short)) + sizeof(short) + unavailables.Sum(x => sizeof(short));
        }
        
    }
    
}