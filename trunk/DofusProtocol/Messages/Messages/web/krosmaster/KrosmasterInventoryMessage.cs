

// Generated on 07/29/2013 23:08:39
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)figures.Count());
            foreach (var entry in figures)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            figures = new Types.KrosmasterFigure[limit];
            for (int i = 0; i < limit; i++)
            {
                 (figures as Types.KrosmasterFigure[])[i] = new Types.KrosmasterFigure();
                 (figures as Types.KrosmasterFigure[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + figures.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}