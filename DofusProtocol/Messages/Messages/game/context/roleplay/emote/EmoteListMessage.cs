

// Generated on 09/26/2016 01:50:00
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
        
        public IEnumerable<byte> emoteIds;
        
        public EmoteListMessage()
        {
        }
        
        public EmoteListMessage(IEnumerable<byte> emoteIds)
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
                 writer.WriteByte(entry);
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
            var emoteIds_ = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 emoteIds_[i] = reader.ReadByte();
            }
            emoteIds = emoteIds_;
        }
        
    }
    
}