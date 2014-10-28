

// Generated on 10/28/2014 16:36:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextRemoveMultipleElementsWithEventsMessage : GameContextRemoveMultipleElementsMessage
    {
        public const uint Id = 6416;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<sbyte> elementEventIds;
        
        public GameContextRemoveMultipleElementsWithEventsMessage()
        {
        }
        
        public GameContextRemoveMultipleElementsWithEventsMessage(IEnumerable<int> id, IEnumerable<sbyte> elementEventIds)
         : base(id)
        {
            this.elementEventIds = elementEventIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var elementEventIds_before = writer.Position;
            var elementEventIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in elementEventIds)
            {
                 writer.WriteSByte(entry);
                 elementEventIds_count++;
            }
            var elementEventIds_after = writer.Position;
            writer.Seek((int)elementEventIds_before);
            writer.WriteUShort((ushort)elementEventIds_count);
            writer.Seek((int)elementEventIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            var elementEventIds_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 elementEventIds_[i] = reader.ReadSByte();
            }
            elementEventIds = elementEventIds_;
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + elementEventIds.Sum(x => sizeof(sbyte));
        }
        
    }
    
}