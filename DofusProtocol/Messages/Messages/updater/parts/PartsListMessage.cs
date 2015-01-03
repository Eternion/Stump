

// Generated on 12/29/2014 21:14:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartsListMessage : Message
    {
        public const uint Id = 1502;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.ContentPart> parts;
        
        public PartsListMessage()
        {
        }
        
        public PartsListMessage(IEnumerable<Types.ContentPart> parts)
        {
            this.parts = parts;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var parts_before = writer.Position;
            var parts_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in parts)
            {
                 entry.Serialize(writer);
                 parts_count++;
            }
            var parts_after = writer.Position;
            writer.Seek((int)parts_before);
            writer.WriteUShort((ushort)parts_count);
            writer.Seek((int)parts_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var parts_ = new Types.ContentPart[limit];
            for (int i = 0; i < limit; i++)
            {
                 parts_[i] = new Types.ContentPart();
                 parts_[i].Deserialize(reader);
            }
            parts = parts_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + parts.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}