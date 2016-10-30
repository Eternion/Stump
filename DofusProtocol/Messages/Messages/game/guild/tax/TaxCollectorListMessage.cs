

// Generated on 10/30/2016 16:20:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorListMessage : AbstractTaxCollectorListMessage
    {
        public const uint Id = 5930;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte nbcollectorMax;
        public IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations;
        
        public TaxCollectorListMessage()
        {
        }
        
        public TaxCollectorListMessage(IEnumerable<Types.TaxCollectorInformations> informations, sbyte nbcollectorMax, IEnumerable<Types.TaxCollectorFightersInformation> fightersInformations)
         : base(informations)
        {
            this.nbcollectorMax = nbcollectorMax;
            this.fightersInformations = fightersInformations;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(nbcollectorMax);
            var fightersInformations_before = writer.Position;
            var fightersInformations_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in fightersInformations)
            {
                 entry.Serialize(writer);
                 fightersInformations_count++;
            }
            var fightersInformations_after = writer.Position;
            writer.Seek((int)fightersInformations_before);
            writer.WriteUShort((ushort)fightersInformations_count);
            writer.Seek((int)fightersInformations_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            nbcollectorMax = reader.ReadSByte();
            if (nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            var limit = reader.ReadUShort();
            var fightersInformations_ = new Types.TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 fightersInformations_[i] = new Types.TaxCollectorFightersInformation();
                 fightersInformations_[i].Deserialize(reader);
            }
            fightersInformations = fightersInformations_;
        }
        
    }
    
}