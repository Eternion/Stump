

// Generated on 02/19/2015 12:09:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var interactiveElements_before = writer.Position;
            var interactiveElements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 interactiveElements_count++;
            }
            var interactiveElements_after = writer.Position;
            writer.Seek((int)interactiveElements_before);
            writer.WriteUShort((ushort)interactiveElements_count);
            writer.Seek((int)interactiveElements_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadVarInt();
            var interactiveElements_ = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 interactiveElements_[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 interactiveElements_[i].Deserialize(reader);
            }
            interactiveElements = interactiveElements_;
        }
        
    }
    
}