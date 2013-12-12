

// Generated on 12/12/2013 16:56:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterReplayWithRecolorRequestMessage : CharacterReplayRequestMessage
    {
        public const uint Id = 6111;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> indexedColor;
        
        public CharacterReplayWithRecolorRequestMessage()
        {
        }
        
        public CharacterReplayWithRecolorRequestMessage(int characterId, IEnumerable<int> indexedColor)
         : base(characterId)
        {
            this.indexedColor = indexedColor;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)indexedColor.Count());
            foreach (var entry in indexedColor)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            indexedColor = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (indexedColor as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + indexedColor.Sum(x => sizeof(int));
        }
        
    }
    
}