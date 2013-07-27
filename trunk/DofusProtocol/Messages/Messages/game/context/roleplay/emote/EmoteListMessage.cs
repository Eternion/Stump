

// Generated on 07/26/2013 22:50:56
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)emoteIds.Count());
            foreach (var entry in emoteIds)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            emoteIds = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (emoteIds as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + emoteIds.Sum(x => sizeof(sbyte));
        }
        
    }
    
}