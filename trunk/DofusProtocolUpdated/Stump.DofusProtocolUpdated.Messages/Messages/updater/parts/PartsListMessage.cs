

// Generated on 12/12/2013 16:57:26
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
            writer.WriteUShort((ushort)parts.Count());
            foreach (var entry in parts)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            parts = new Types.ContentPart[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parts as Types.ContentPart[])[i] = new Types.ContentPart();
                 (parts as Types.ContentPart[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + parts.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}