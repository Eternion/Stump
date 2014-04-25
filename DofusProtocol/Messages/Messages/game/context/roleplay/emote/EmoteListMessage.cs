

// Generated on 03/02/2014 20:42:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class EmoteListMessage : Message
    {
        public const uint Id = 5689;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<sbyte> emoteIds;
        
        public EmoteListMessage()
        {
        }
        
        public EmoteListMessage(IEnumerable<sbyte> emoteIds)
        {
            this.emoteIds = emoteIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var emoteIds_before = writer.Position;
            var emoteIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in emoteIds)
            {
                 writer.WriteSByte(entry);
                 emoteIds_count++;
            }
            var emoteIds_after = writer.Position;
            writer.Seek((int)emoteIds_before);
            writer.WriteUShort((ushort)emoteIds_count);
            writer.Seek((int)emoteIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var emoteIds_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 emoteIds_[i] = reader.ReadSByte();
            }
            emoteIds = emoteIds_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + emoteIds.Sum(x => sizeof(sbyte));
        }
        
    }
    
}