

// Generated on 07/29/2013 23:07:40
using System;
using System.Collections.Generic;
using System.Linq;
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