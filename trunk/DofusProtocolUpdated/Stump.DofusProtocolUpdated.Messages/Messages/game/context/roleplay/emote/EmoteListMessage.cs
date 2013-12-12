

// Generated on 12/12/2013 16:57:00
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
            writer.WriteUShort((ushort)emoteIds.Count());
            foreach (var entry in emoteIds)
            {
                 writer.WriteByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            emoteIds = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (emoteIds as byte[])[i] = reader.ReadByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + emoteIds.Sum(x => sizeof(byte));
        }
        
    }
    
}