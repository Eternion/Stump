

// Generated on 12/12/2013 16:56:55
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
            writer.WriteUShort((ushort)elementEventIds.Count());
            foreach (var entry in elementEventIds)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            elementEventIds = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (elementEventIds as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + elementEventIds.Sum(x => sizeof(sbyte));
        }
        
    }
    
}