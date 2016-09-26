

// Generated on 09/26/2016 01:50:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareListMessage : Message
    {
        public const uint Id = 6661;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.DareInformations> dares;
        
        public DareListMessage()
        {
        }
        
        public DareListMessage(IEnumerable<Types.DareInformations> dares)
        {
            this.dares = dares;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var dares_before = writer.Position;
            var dares_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in dares)
            {
                 entry.Serialize(writer);
                 dares_count++;
            }
            var dares_after = writer.Position;
            writer.Seek((int)dares_before);
            writer.WriteUShort((ushort)dares_count);
            writer.Seek((int)dares_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var dares_ = new Types.DareInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 dares_[i] = new Types.DareInformations();
                 dares_[i].Deserialize(reader);
            }
            dares = dares_;
        }
        
    }
    
}