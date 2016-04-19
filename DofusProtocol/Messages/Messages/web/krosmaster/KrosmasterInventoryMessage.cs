

// Generated on 04/19/2016 10:17:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KrosmasterInventoryMessage : Message
    {
        public const uint Id = 6350;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.KrosmasterFigure> figures;
        
        public KrosmasterInventoryMessage()
        {
        }
        
        public KrosmasterInventoryMessage(IEnumerable<Types.KrosmasterFigure> figures)
        {
            this.figures = figures;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var figures_before = writer.Position;
            var figures_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in figures)
            {
                 entry.Serialize(writer);
                 figures_count++;
            }
            var figures_after = writer.Position;
            writer.Seek((int)figures_before);
            writer.WriteUShort((ushort)figures_count);
            writer.Seek((int)figures_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var figures_ = new Types.KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++)
            {
                 figures_[i] = new Types.KrosmasterFigure();
                 figures_[i].Deserialize(reader);
            }
            figures = figures_;
        }
        
    }
    
}