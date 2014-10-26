

// Generated on 10/26/2014 23:29:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterSelectionWithRecolorMessage : CharacterSelectionMessage
    {
        public const uint Id = 6075;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> indexedColor;
        
        public CharacterSelectionWithRecolorMessage()
        {
        }
        
        public CharacterSelectionWithRecolorMessage(int id, IEnumerable<int> indexedColor)
         : base(id)
        {
            this.indexedColor = indexedColor;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var indexedColor_before = writer.Position;
            var indexedColor_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in indexedColor)
            {
                 writer.WriteInt(entry);
                 indexedColor_count++;
            }
            var indexedColor_after = writer.Position;
            writer.Seek((int)indexedColor_before);
            writer.WriteUShort((ushort)indexedColor_count);
            writer.Seek((int)indexedColor_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var indexedColor_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 indexedColor_[i] = reader.ReadInt();
            }
            indexedColor = indexedColor_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + indexedColor.Sum(x => sizeof(int));
        }
        
    }
    
}