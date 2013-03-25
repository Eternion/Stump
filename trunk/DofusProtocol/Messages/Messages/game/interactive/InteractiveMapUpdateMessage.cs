
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveMapUpdateMessage : Message
    {
        public const uint Id = 5002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.InteractiveElement> interactiveElements;
        
        public InteractiveMapUpdateMessage()
        {
        }
        
        public InteractiveMapUpdateMessage(IEnumerable<Types.InteractiveElement> interactiveElements)
        {
            this.interactiveElements = interactiveElements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)interactiveElements.Count());
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            interactiveElements = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (interactiveElements as Types.InteractiveElement[])[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 (interactiveElements as Types.InteractiveElement[])[i].Deserialize(reader);
            }
        }
        
    }
    
}