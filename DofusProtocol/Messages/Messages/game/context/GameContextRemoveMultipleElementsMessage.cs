

// Generated on 12/20/2015 16:36:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextRemoveMultipleElementsMessage : Message
    {
        public const uint Id = 252;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<double> id;
        
        public GameContextRemoveMultipleElementsMessage()
        {
        }
        
        public GameContextRemoveMultipleElementsMessage(IEnumerable<double> id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var id_before = writer.Position;
            var id_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in id)
            {
                 writer.WriteDouble(entry);
                 id_count++;
            }
            var id_after = writer.Position;
            writer.Seek((int)id_before);
            writer.WriteUShort((ushort)id_count);
            writer.Seek((int)id_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var id_ = new double[limit];
            for (int i = 0; i < limit; i++)
            {
                 id_[i] = reader.ReadDouble();
            }
            id = id_;
        }
        
    }
    
}